if(!self.define){let e,s={};const r=(r,i)=>(r=new URL(r+".js",i).href,s[r]||new Promise((s=>{if("document"in self){const e=document.createElement("script");e.src=r,e.onload=s,document.head.appendChild(e)}else e=r,importScripts(r),s()})).then((()=>{let e=s[r];if(!e)throw new Error(`Module ${r} didn’t register its module`);return e})));self.define=(i,n)=>{const o=e||("document"in self?document.currentScript.src:"")||location.href;if(s[o])return;let t={};const l=e=>r(e,o),u={module:{uri:o},exports:t,require:l};s[o]=Promise.all(i.map((e=>u[e]||l(e)))).then((e=>(n(...e),t)))}}define(["./workbox-2d118ab0"],(function(e){"use strict";e.setCacheNameDetails({prefix:"file-manager"}),self.addEventListener("message",(e=>{e.data&&"SKIP_WAITING"===e.data.type&&self.skipWaiting()})),e.precacheAndRoute([{url:"/css/app.3853b14d.css",revision:null},{url:"/index.html",revision:"34be68d55bae51b0039da6a4faa1c574"},{url:"/js/app.27529be6.js",revision:null},{url:"/js/chunk-vendors.49e2ba20.js",revision:null},{url:"/manifest.json",revision:"cf67e697ab613bb74d70dc35280383d6"},{url:"/robots.txt",revision:"735ab4f94fbcd57074377afca324c813"}],{})}));
//# sourceMappingURL=service-worker.js.map