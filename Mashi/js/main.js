requirejs.config({
  baseUrl: '/js/lib',
  paths: {
    app: '../app',
    'async': 'async',
    'goog': 'goog',
    'propertyParser' : 'propertyParser'
  },
  shim: {
    'underscore': {
      exports: '_'
    }
  }
});

define(
  'gmaps',
  ['async!https://maps.googleapis.com/maps/api/js?libraries=geometry&amp;sensor=false&key=AIzaSyCu_aP2mLkcaR6S7vfCjOxlcm5-GuBt1ZM'],
function(){
  return window.google.maps;
});

requirejs([
  'app/route',
  'gmaps',
],

function(route, gmaps) {
  // all ready!
});
