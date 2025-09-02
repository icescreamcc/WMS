 const jsonp=(url:string)=> {
    return new Promise((resolve, reject) => {
      const callbackName:string = `jsonp_callback_${Date.now()}`;
      const script = document.createElement('script');
      script.src = url + (url.indexOf('?') >= 0 ? '&' : '?') + 'callback=' + callbackName;
      script.onerror = reject;
      window[callbackName] = function (data) {
        resolve(data);
        delete window[callbackName];
        script.parentNode.removeChild(script);
      }; 
      document.body.appendChild(script);
});
}

export default jsonp