define([
  './gmaps',
  './underscore',
  './points',
  './filters',
  './styles',
  './animation'],
function(gmaps, _, points, filters, styles, animateRoute){

  function Route(options) {
    this.options = this.extend(this._options, options);
   this.init();
  }

  Route.prototype = {

    // default options
    _options: {
      initializeFilters: true,
      animate: false
    },

    map: {},

    mapTileListener: null,

    coordinates: [],

    line: {},

    enabledFilters: {},

    init: function(){
      this.enabledFilters = (this.options.initializeFilters ? filters : {});
      this.parseJSON(points);
    },
 init123: function(){
     // this.enabledFilters = (this.options.initializeFilters ? filters : {});
      this.parseJSON(points);
    },
    parseJSON: function(data){

      this.coordinates = data.map(function(item){
        return {
          lat: item.latitude,
          lng: item.longitude,
          timestamp: item.timestamp,
          googLatLng: new gmaps.LatLng(item.latitude, item.longitude)
        }
      });

      this.drawMap();
    },

    drawMap: function() {

      var self = this,
          forEach = Array.prototype.forEach;

      self.map = new gmaps.Map(document.querySelector(".map"), {
        center: new gmaps.LatLng(51.512361, -0.1404834),
        zoom: 13,
        mapTypeId: gmaps.MapTypeId.ROADMAP,
        styles: styles,
        panControl: false,
        zoomControl: false,
        mapTypeControl: false,
        streetViewControl : false,
        scrollwheel: false,
        zoomControlOptions : {
          position: gmaps.ControlPosition.LEFT_BOTTOM,
          style: gmaps.ZoomControlStyle.LARGE
        }
      });

      // Wait map to be fully loaded before set the markers
      self.mapTileListener = gmaps.event.addListener(self.map, 'tilesloaded', function(){
        self.setMarkers();
        gmaps.event.removeListener(self.tileListener);
      });

    },

    setMarkers: function() {

      var self = this,
          startMarker, midMarker, midMarker2, midMarker3, endMarker, pin;

      pin = new gmaps.MarkerImage('images/pin.png', null, null, null, new gmaps.Size(20,25));

      startMarker = new gmaps.Marker({
        position: self.coordinates[0].googLatLng,
        icon: pin,
        map: self.map,
        //animation: google.maps.Animation.DROP
      });

	  midMarker = new gmaps.Marker({
        position: self.coordinates[100].googLatLng,
        icon: pin,
        map: self.map,
        //animation: google.maps.Animation.DROP
      });
	  midMarker2 = new gmaps.Marker({
        position: self.coordinates[130].googLatLng,
        icon: pin,
        map: self.map,
        //animation: google.maps.Animation.DROP
      });
	  midMarker3 = new gmaps.Marker({
        position: self.coordinates[180].googLatLng,
        icon: pin,
        map: self.map,
        //animation: google.maps.Animation.DROP
      });
	  
      endMarker = new google.maps.Marker({
        position: self.coordinates[self.coordinates.length-1].googLatLng,
        icon: pin,
        map: self.map,
        //animation: google.maps.Animation.DROP
      });

      //self.updateRoutes();
    },

    updateRoutes: function() {

      var pathCoordinates = _.pluck(this.normalizeCoordinates(), "googLatLng");

      if(this.options.animate) {
        this.enabledFilters = filters;
        pathCoordinates = _.pluck(this.normalizeCoordinates(), "googLatLng");
        animateRoute(pathCoordinates, this.map);
        return;
      }

      this.line = new gmaps.Polyline({
        path: pathCoordinates,
        geodesic: false,
        strokeColor: '#f1d32e',
        strokeOpacity: 1,
        strokeWeight: 2
      });

      this.line.setMap(this.map);

    },

    // Remove potentially erroneous points
    normalizeCoordinates: function() {

      var self = this;
      var filtersList = _.keys(self.enabledFilters);

      return _.reduce(filtersList, function(memo, filter) {
        return self.enabledFilters[filter](memo);
      }, self.coordinates);

    },

    playAnimation: function(tt) {
//alert(tt)
      if (this.line.setMap) {
        this.line.setMap(null);
      }

      this.options.animate = true;
      this.updateRoutes();

    },

    extend: function(a, b) {

      for (var key in b) {
        if (b.hasOwnProperty(key)) {
          a[key] = b[key];
        }
      }
      return a;

    }

  }

  return Route;

});
