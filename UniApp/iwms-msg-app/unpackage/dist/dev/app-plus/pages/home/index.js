"use weex:vue";

if (typeof Promise !== 'undefined' && !Promise.prototype.finally) {
  Promise.prototype.finally = function(callback) {
    const promise = this.constructor
    return this.then(
      value => promise.resolve(callback()).then(() => value),
      reason => promise.resolve(callback()).then(() => {
        throw reason
      })
    )
  }
};

if (typeof uni !== 'undefined' && uni && uni.requireGlobal) {
  const global = uni.requireGlobal()
  ArrayBuffer = global.ArrayBuffer
  Int8Array = global.Int8Array
  Uint8Array = global.Uint8Array
  Uint8ClampedArray = global.Uint8ClampedArray
  Int16Array = global.Int16Array
  Uint16Array = global.Uint16Array
  Int32Array = global.Int32Array
  Uint32Array = global.Uint32Array
  Float32Array = global.Float32Array
  Float64Array = global.Float64Array
  BigInt64Array = global.BigInt64Array
  BigUint64Array = global.BigUint64Array
};


(() => {
  var __create = Object.create;
  var __defProp = Object.defineProperty;
  var __getOwnPropDesc = Object.getOwnPropertyDescriptor;
  var __getOwnPropNames = Object.getOwnPropertyNames;
  var __getProtoOf = Object.getPrototypeOf;
  var __hasOwnProp = Object.prototype.hasOwnProperty;
  var __commonJS = (cb, mod) => function __require() {
    return mod || (0, cb[__getOwnPropNames(cb)[0]])((mod = { exports: {} }).exports, mod), mod.exports;
  };
  var __copyProps = (to, from, except, desc) => {
    if (from && typeof from === "object" || typeof from === "function") {
      for (let key of __getOwnPropNames(from))
        if (!__hasOwnProp.call(to, key) && key !== except)
          __defProp(to, key, { get: () => from[key], enumerable: !(desc = __getOwnPropDesc(from, key)) || desc.enumerable });
    }
    return to;
  };
  var __toESM = (mod, isNodeMode, target) => (target = mod != null ? __create(__getProtoOf(mod)) : {}, __copyProps(
    // If the importer is in node compatibility mode or this is not an ESM
    // file that has been converted to a CommonJS file using a Babel-
    // compatible transform (i.e. "__esModule" has not been set), then set
    // "default" to the CommonJS "module.exports" for node compatibility.
    isNodeMode || !mod || !mod.__esModule ? __defProp(target, "default", { value: mod, enumerable: true }) : target,
    mod
  ));

  // vue-ns:vue
  var require_vue = __commonJS({
    "vue-ns:vue"(exports, module) {
      module.exports = Vue;
    }
  });

  // ../../../project/app-demo/app-vue-demo/iwms-msg-app/unpackage/dist/dev/.nvue/pages/home/index.js
  var import_vue = __toESM(require_vue());
  function formatAppLog(type, filename, ...args) {
    if (uni.__log__) {
      uni.__log__(type, filename, ...args);
    } else {
      console[type].apply(console, [...args, filename]);
    }
  }
  var _style_0 = { "page": { "": { "backgroundColor": "#6b96a1", "height": 100, "width": 100, "position": "absolute" } }, "page-head": { ".page ": { "textAlign": "center", "color": "#e6a23c", "paddingTop": 5, "paddingRight": 0, "paddingBottom": 5, "paddingLeft": 0, "backgroundColor": "#00ffb7" } }, "page-body": { ".page ": { "paddingTop": 0, "paddingRight": 10, "paddingBottom": 0, "paddingLeft": 10, "overflowY": "scroll" } }, "body-item": { ".page .page-body ": { "position": "relative" } }, "item-icon": { ".page .page-body .body-item ": { "height": 40, "position": "absolute", "right": 1 } }, "item-info": { ".page .page-body .body-item ": { "marginTop": 10, "width": 90, "paddingTop": 5, "paddingRight": 0, "paddingBottom": 5, "paddingLeft": 0, "backgroundColor": "#ffffff", "borderRadius": 5 } }, "info-title": { ".page .page-body .body-item .item-info ": { "paddingTop": 0, "paddingRight": 5, "paddingBottom": 0, "paddingLeft": 5, "color": "#e6a23c", "fontWeight": "600", "marginBottom": 5, "fontSize": 13 } }, "info-date": { ".page .page-body .body-item .item-info ": { "paddingTop": 0, "paddingRight": 5, "paddingBottom": 0, "paddingLeft": 5, "fontSize": 12, "textAlign": "right", "marginBottom": 10 } }, "info-content": { ".page .page-body .body-item .item-info ": { "paddingTop": 0, "paddingRight": 5, "paddingBottom": 0, "paddingLeft": 5, "fontSize": 13 } } };
  var _export_sfc = (sfc, props) => {
    const target = sfc.__vccOpts || sfc;
    for (const [key, val] of props) {
      target[key] = val;
    }
    return target;
  };
  var _sfc_main = {
    data() {
      return {
        windowWidth: 366,
        windowHeight: 342,
        connected: false,
        connecting: false,
        msg: false,
        roomId: "",
        msgContent: []
      };
    },
    onReady() {
      uni.getSystemInfo().then((res) => {
        this.windowWidth = res.windowWidth;
        this.windowHeight = res.windowHeight;
      });
      this.connect();
    },
    computed: {
      showMsg() {
        if (this.connected) {
          if (this.msg) {
            return "\u6536\u5230\u6D88\u606F\uFF1A" + this.msg;
          } else {
            return "\u7B49\u5F85\u63A5\u6536\u6D88\u606F";
          }
        } else {
          return "\u5C1A\u672A\u8FDE\u63A5";
        }
      }
    },
    methods: {
      notificationsPermission() {
        var main = plus.android.runtimeMainActivity();
        main.getPackageName();
        var NotificationManagerCompat = plus.android.importClass(
          "androidx.core.app.NotificationManagerCompat"
        );
        var isEnabled = NotificationManagerCompat.from(main).areNotificationsEnabled();
        return isEnabled;
      },
      pushMsg(title, msg) {
        let content = msg;
        let option = {
          "cover": false,
          "when": /* @__PURE__ */ new Date(),
          "title": title,
          "sound": "system"
        };
        let body = {
          "id": "1234",
          "key": "key12345"
        };
        let payload = JSON.stringify(body);
        plus.push.createMessage(content, payload, option);
      },
      connect() {
        formatAppLog("log", "at pages/home/index.nvue:120", "socket\u8FDE\u63A5", this.connected);
        if (this.connected || this.connecting) {
          uni.showToast({
            title: "\u6B63\u5728\u8FDE\u63A5\u6216\u8005\u5DF2\u7ECF\u8FDE\u63A5\uFF0C\u8BF7\u52FF\u91CD\u590D\u8FDE\u63A5",
            icon: "none"
          });
          return false;
        }
        this.connecting = true;
        uni.showLoading({
          title: "Socket\u8FDE\u63A5\u4E2D..."
        });
        uni.connectSocket({
          url: "ws://10.24.78.64:8030",
          data() {
            return {
              msg: "Hello"
            };
          },
          success(res) {
          },
          fail(err) {
            uni.showToast({
              title: `Socket\u63A5\u53E3\u8C03\u7528\u5931\u8D25,${err}`,
              icon: "none"
            });
          }
        });
        uni.onSocketOpen((res) => {
          this.connecting = false;
          this.connected = true;
          uni.hideLoading();
          formatAppLog("log", "at pages/home/index.nvue:163", "\u4E0E\u670D\u52A1\u5668\u8FDE\u63A5\u6210\u529F");
          uni.showToast({
            icon: "none",
            title: "\u4E0E\u670D\u52A1\u5668\u8FDE\u63A5\u6210\u529F"
          });
        });
        uni.onSocketError((err) => {
          this.connecting = false;
          this.connected = false;
          uni.hideLoading();
          uni.showModal({
            content: `\u4E0E\u670D\u52A1\u5668\u8FDE\u63A5\u5931\u8D25:${JSON.stringify(err)}`,
            confirmText: "\u91CD\u65B0\u8FDE\u63A5",
            confirmColor: "#007aff",
            success: (res) => {
              if (res.confirm) {
                this.connect();
              }
            }
          });
        });
        uni.onSocketMessage((res) => {
          this.msg = res.data;
          let data = JSON.parse(res.data);
          this.msgContent.push(data);
          this.pushMsg(data.Name, data.Message);
          uni.vibrateLong();
        });
        uni.onSocketClose((res) => {
          this.connected = false;
          this.startRecive = false;
          this.msg = false;
        });
      },
      send() {
        uni.sendSocketMessage({
          data: "from " + platform + " : " + parseInt(Math.random() * 1e4).toString(),
          success(res) {
            formatAppLog("log", "at pages/home/index.nvue:201", res);
          },
          fail(err) {
            formatAppLog("log", "at pages/home/index.nvue:204", err);
          }
        });
      },
      close() {
        uni.closeSocket();
      },
      connectSocket2() {
        let connectString = "ws://10.24.78.64:8030";
        let socketClient = new WebSocket(connectString);
        socketClient.addEventListener("open", (event) => {
          if (event.isTrusted && event.type == "open") {
            formatAppLog("log", "at pages/home/index.nvue:216", "event", event);
            uni.showToast({
              title: "\u8FDE\u63A5\u6210\u529F",
              icon: "none"
            });
            socketClient.addEventListener("message", (event2) => {
              let data = JSON.parse(event2.data);
              formatAppLog("log", "at pages/home/index.nvue:223", "event", data);
              uni.showToast({
                title: data.Message,
                icon: "none"
              });
            });
          }
        });
      }
    }
  };
  function _sfc_render(_ctx, _cache, $props, $setup, $data, $options) {
    return (0, import_vue.openBlock)(), (0, import_vue.createElementBlock)("scroll-view", {
      scrollY: true,
      showScrollbar: true,
      enableBackToTop: true,
      bubble: "true",
      style: { flexDirection: "column" }
    }, [
      (0, import_vue.createElementVNode)(
        "div",
        {
          class: "page",
          style: (0, import_vue.normalizeStyle)({ width: $data.windowWidth, height: $data.windowHeight })
        },
        [
          (0, import_vue.createElementVNode)(
            "div",
            {
              class: "page-head",
              style: (0, import_vue.normalizeStyle)({ width: $data.windowWidth })
            },
            [
              (0, import_vue.createElementVNode)("u-text", null, "\u6B22\u8FCE\u4F7F\u7528IWMS\u6D88\u606F\u901A\u77E5APP")
            ],
            4
            /* STYLE */
          ),
          (0, import_vue.createElementVNode)(
            "div",
            {
              class: "page-body",
              style: (0, import_vue.normalizeStyle)({ width: $data.windowWidth, height: $data.windowHeight * 0.8 })
            },
            [
              (0, import_vue.createElementVNode)("scroll-view", { scrollY: "true" }, [
                ((0, import_vue.openBlock)(true), (0, import_vue.createElementBlock)(
                  import_vue.Fragment,
                  null,
                  (0, import_vue.renderList)($data.msgContent, (item) => {
                    return (0, import_vue.openBlock)(), (0, import_vue.createElementBlock)(
                      "div",
                      {
                        class: "body-item",
                        style: (0, import_vue.normalizeStyle)({ width: $data.windowWidth })
                      },
                      [
                        (0, import_vue.createElementVNode)(
                          "view",
                          {
                            "flexDirection:row": "",
                            style: (0, import_vue.normalizeStyle)({ width: $data.windowWidth * 0.99 })
                          },
                          [
                            (0, import_vue.createElementVNode)("img", {
                              class: "item-icon",
                              style: { "height": "40px" },
                              src: "/static/image/xiaoxi1.png"
                            }),
                            (0, import_vue.createElementVNode)(
                              "div",
                              {
                                class: "item-info",
                                style: (0, import_vue.normalizeStyle)({ width: $data.windowWidth })
                              },
                              [
                                (0, import_vue.createElementVNode)("div", { class: "info-date" }, [
                                  (0, import_vue.createElementVNode)(
                                    "u-text",
                                    null,
                                    (0, import_vue.toDisplayString)(item.Date),
                                    1
                                    /* TEXT */
                                  )
                                ]),
                                (0, import_vue.createElementVNode)("div", { class: "info-title" }, [
                                  (0, import_vue.createElementVNode)(
                                    "u-text",
                                    null,
                                    (0, import_vue.toDisplayString)(item.Name),
                                    1
                                    /* TEXT */
                                  )
                                ]),
                                (0, import_vue.createElementVNode)("div", { class: "info-content" }, [
                                  (0, import_vue.createElementVNode)(
                                    "u-text",
                                    null,
                                    (0, import_vue.toDisplayString)(item.Message),
                                    1
                                    /* TEXT */
                                  )
                                ])
                              ],
                              4
                              /* STYLE */
                            )
                          ],
                          4
                          /* STYLE */
                        )
                      ],
                      4
                      /* STYLE */
                    );
                  }),
                  256
                  /* UNKEYED_FRAGMENT */
                ))
              ])
            ],
            4
            /* STYLE */
          )
        ],
        4
        /* STYLE */
      )
    ]);
  }
  var index = /* @__PURE__ */ _export_sfc(_sfc_main, [["render", _sfc_render], ["styles", [_style_0]], ["__file", "D:/project/app-demo/app-vue-demo/iwms-msg-app/pages/home/index.nvue"]]);

  // <stdin>
  var webview = plus.webview.currentWebview();
  if (webview) {
    const __pageId = parseInt(webview.id);
    const __pagePath = "pages/home/index";
    let __pageQuery = {};
    try {
      __pageQuery = JSON.parse(webview.__query__);
    } catch (e) {
    }
    index.mpType = "page";
    const app = Vue.createPageApp(index, { $store: getApp({ allowDefault: true }).$store, __pageId, __pagePath, __pageQuery });
    app.provide("__globalStyles", Vue.useCssStyles([...__uniConfig.styles, ...index.styles || []]));
    app.mount("#root");
  }
})();
