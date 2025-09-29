import { openBlock, createElementBlock, createElementVNode, normalizeStyle, Fragment, renderList, toDisplayString } from "vue";
function formatAppLog(type, filename, ...args) {
  if (uni.__log__) {
    uni.__log__(type, filename, ...args);
  } else {
    console[type].apply(console, [...args, filename]);
  }
}
const _style_0 = { "page": { "": { "backgroundColor": "#6b96a1", "height": 100, "width": 100, "position": "absolute" } }, "page-head": { ".page ": { "textAlign": "center", "color": "#e6a23c", "paddingTop": 5, "paddingRight": 0, "paddingBottom": 5, "paddingLeft": 0, "backgroundColor": "#00ffb7" } }, "page-body": { ".page ": { "paddingTop": 0, "paddingRight": 10, "paddingBottom": 0, "paddingLeft": 10, "overflowY": "scroll" } }, "body-item": { ".page .page-body ": { "position": "relative" } }, "item-icon": { ".page .page-body .body-item ": { "height": 40, "position": "absolute", "right": 1 } }, "item-info": { ".page .page-body .body-item ": { "marginTop": 10, "width": 90, "paddingTop": 5, "paddingRight": 0, "paddingBottom": 5, "paddingLeft": 0, "backgroundColor": "#ffffff", "borderRadius": 5 } }, "info-title": { ".page .page-body .body-item .item-info ": { "paddingTop": 0, "paddingRight": 5, "paddingBottom": 0, "paddingLeft": 5, "color": "#e6a23c", "fontWeight": "600", "marginBottom": 5, "fontSize": 13 } }, "info-date": { ".page .page-body .body-item .item-info ": { "paddingTop": 0, "paddingRight": 5, "paddingBottom": 0, "paddingLeft": 5, "fontSize": 12, "textAlign": "right", "marginBottom": 10 } }, "info-content": { ".page .page-body .body-item .item-info ": { "paddingTop": 0, "paddingRight": 5, "paddingBottom": 0, "paddingLeft": 5, "fontSize": 13 } } };
const _export_sfc = (sfc, props) => {
  const target = sfc.__vccOpts || sfc;
  for (const [key, val] of props) {
    target[key] = val;
  }
  return target;
};
const _sfc_main = {
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
          return "收到消息：" + this.msg;
        } else {
          return "等待接收消息";
        }
      } else {
        return "尚未连接";
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
      formatAppLog("log", "at pages/home/index.nvue:120", "socket连接", this.connected);
      if (this.connected || this.connecting) {
        uni.showToast({
          title: "正在连接或者已经连接，请勿重复连接",
          icon: "none"
        });
        return false;
      }
      this.connecting = true;
      uni.showLoading({
        title: "Socket连接中..."
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
            title: `Socket接口调用失败,${err}`,
            icon: "none"
          });
        }
      });
      uni.onSocketOpen((res) => {
        this.connecting = false;
        this.connected = true;
        uni.hideLoading();
        formatAppLog("log", "at pages/home/index.nvue:163", "与服务器连接成功");
        uni.showToast({
          icon: "none",
          title: "与服务器连接成功"
        });
      });
      uni.onSocketError((err) => {
        this.connecting = false;
        this.connected = false;
        uni.hideLoading();
        uni.showModal({
          content: `与服务器连接失败:${JSON.stringify(err)}`,
          confirmText: "重新连接",
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
            title: "连接成功",
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
  return openBlock(), createElementBlock("scroll-view", {
    scrollY: true,
    showScrollbar: true,
    enableBackToTop: true,
    bubble: "true",
    style: { flexDirection: "column" }
  }, [
    createElementVNode(
      "div",
      {
        class: "page",
        style: normalizeStyle({ width: $data.windowWidth, height: $data.windowHeight })
      },
      [
        createElementVNode(
          "div",
          {
            class: "page-head",
            style: normalizeStyle({ width: $data.windowWidth })
          },
          [
            createElementVNode("u-text", null, "欢迎使用IWMS消息通知APP")
          ],
          4
          /* STYLE */
        ),
        createElementVNode(
          "div",
          {
            class: "page-body",
            style: normalizeStyle({ width: $data.windowWidth, height: $data.windowHeight * 0.8 })
          },
          [
            createElementVNode("scroll-view", { scrollY: "true" }, [
              (openBlock(true), createElementBlock(
                Fragment,
                null,
                renderList($data.msgContent, (item) => {
                  return openBlock(), createElementBlock(
                    "div",
                    {
                      class: "body-item",
                      style: normalizeStyle({ width: $data.windowWidth })
                    },
                    [
                      createElementVNode(
                        "view",
                        {
                          "flexDirection:row": "",
                          style: normalizeStyle({ width: $data.windowWidth * 0.99 })
                        },
                        [
                          createElementVNode("img", {
                            class: "item-icon",
                            style: { "height": "40px" },
                            src: "/static/image/xiaoxi1.png"
                          }),
                          createElementVNode(
                            "div",
                            {
                              class: "item-info",
                              style: normalizeStyle({ width: $data.windowWidth })
                            },
                            [
                              createElementVNode("div", { class: "info-date" }, [
                                createElementVNode(
                                  "u-text",
                                  null,
                                  toDisplayString(item.Date),
                                  1
                                  /* TEXT */
                                )
                              ]),
                              createElementVNode("div", { class: "info-title" }, [
                                createElementVNode(
                                  "u-text",
                                  null,
                                  toDisplayString(item.Name),
                                  1
                                  /* TEXT */
                                )
                              ]),
                              createElementVNode("div", { class: "info-content" }, [
                                createElementVNode(
                                  "u-text",
                                  null,
                                  toDisplayString(item.Message),
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
const index = /* @__PURE__ */ _export_sfc(_sfc_main, [["render", _sfc_render], ["styles", [_style_0]], ["__file", "D:/project/app-demo/app-vue-demo/iwms-msg-app/pages/home/index.nvue"]]);
export {
  index as default
};
