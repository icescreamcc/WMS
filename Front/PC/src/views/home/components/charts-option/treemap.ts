const getLevelOption=()=> {
    return [
         {
            itemStyle: {
              borderWidth: 0,
              gapWidth: 8 
            } 
          },
          {
            itemStyle: {
              gapWidth: 2
            } 
          },
          {
            colorSaturation: [0.35, 0.5],
            itemStyle: {
              gapWidth: 1,
              borderColorSaturation: 0.6
            } 
          }
    ];
  }

const getOption=(echarts:any,mapdata:any):any=> {
 let formatUtil = echarts.format; 
  return { 
        darkMode: 'auto',
        colorBy: 'series',
        color:['#5470c6','#016a2f','#91cc75','#c01509','#fac858','#673108','#73c0de','#20041e','#3ba272','#fc8452','#9a60b4','#ea7ccc'],
        gradientColor:['#f6efa6','#d88273','#bf444c'],
        aria:{
            decal:{
                decals:[
                    {
                        color: 'rgba(0, 0, 0, 0.2)',
                        symbolSize: 1,
                        rotation: 0.5235987755982988,
                        dashArrayX:[1,0],
                        dashArrayY:[2,5]
                    },
                    {
                        color: 'rgba(0, 0, 0, 0.2)',
                        symbol: 'circle',
                        symbolSize: 0.8, 
                        dashArrayX:[[8,8],[0,8,8,0]],
                        dashArrayY:[6,0]
                    },
                    {
                        color: 'rgba(0, 0, 0, 0.2)', 
                        rotation: -0.7853981633974483,
                        dashArrayX:[1,0],
                        dashArrayY:[4,3]
                    },
                    {
                        color: 'rgba(0, 0, 0, 0.2)',  
                        dashArrayX:[[6,6],[0,6,6,0]],
                        dashArrayY:[6,0]
                    },
                ]
            }
        },
        stateAnimation:{
            duration: 300,
            easing: 'cubicOut'
        },
        animation: 'auto',
        animationDuration: 1000,
        animationDurationUpdate: 500,
        animationEasing: 'cubicInOut',
        animationEasingUpdate: 'cubicInOut',
        animationThreshold: 2000,
        progressiveThreshold: 3000,
        progressive: 400,
        hoverLayerThreshold: 3000,
        useUTC: false,
        title:[
            {
                text: '仓库货品全览',
                left: 'center',
                z: 6,
                show: true,
                target: 'blank',
                subtext:'' ,
                subtarget: 'blank',
                top: 0,
                backgroundColor: 'rgba(0,0,0,0)',
                borderColor: '#ccc',
                borderWidth: 0,
                padding: 5,
                itemGap: 10,
                textStyle:{
                    color:'#464646'
                },
                subtextStyle:{
                    color: '#6E7079'
                }
            }
        ], 
        axisPointer:[
            {
                show: 'auto',
                z: 50,
                type: 'line',
                snap: false,
                triggerTooltip: true,
                triggerEmphasis: true,
                value: null,
                status: null,
                animation: null,
                animationDurationUpdate: 200,
                lineStyle:{
                    color: '#B9BEC9',
                    width: 1,
                    type: 'dashed',
                },
                shadowStyle:{
                    color: 'rgba(210,219,238,0.2)',
                },
                label:{
                    show: true,
                    formatter: null,
                    precision: 'auto',
                    margin: 3,
                    color: '#fff',
                    padding:[5,7,5,7] , 
                    backgroundColor: 'auto',
                    borderColor: null,
                    borderWidth: 0,
                    borderRadius: 3,
                },
                handle:{
                    show: false,
                    icon: 'M10.7,11.9v-1.3H9.3v1.3c-4.9,0.3-8.8,4.4-8.8,9.4c0,5,3.9,9.1,8.8,9.4h1.3c4.9-0.3,8.8-4.4,8.8-9.4C19.5,16.3,15.6,12.2,10.7,11.9z M13.3,24.4H6.7v-1.2h6.6z M13.3,22H6.7v-1.2h6.6z M13.3,19.6H6.7v-1.2h6.6z',
                    size: 45,
                    margin: 50,
                    color: '#333',
                    shadowBlur: 3,
                    shadowColor: '#aaa',
                    shadowOffsetX: 0,
                    shadowOffsetY: 2,
                    throttle: 40
                }
            }
        ],
        tooltip:[
            {
                formatter: function (info:any) {
                    var value = info.value;
                    var treePathInfo = info.treePathInfo;
                    var treePath = [];
                    for (var i = 1; i < treePathInfo.length; i++) { 
                      treePath.push(treePathInfo[i].name);
                    } 
                    return [
                      `<div class="tooltip-title">${formatUtil.encodeHTML(treePath.join('/'))}</div>` , 
                      `${info.data.props?`(${info.data.props})`:''}${formatUtil.addCommas(value)+info.data.remark}` 
                    ].join('');
                  },
                z: 60,
                show: true,
                showContent: true,
                trigger: 'item',
                triggerOn: 'mousemove|click',
                alwaysShowContent: false,
                displayMode: 'single',
                renderMode: 'auto',
                confine: null,
                showDelay: 0,
                hideDelay: 100,
                transitionDuration: 0.4,
                enterable: false,
                backgroundColor: '#fff',
                shadowBlur: 10,
                shadowColor: 'rgba(0, 0, 0, .2)',
                shadowOffsetX: 1,
                shadowOffsetY: 2,
                borderRadius: 4,
                borderWidth: 1,
                padding: null,
                extraCssText: ''
            }
        ],
        series: [
        {
            name: '货品',
            type: 'treemap',
            visibleMin: 300,
            label: {
              show: true,
              formatter: '{b}'
            },
            itemStyle: {
              borderColor: '#fff'
            },
            levels: getLevelOption(),
            data:mapdata 
        }
        ] ,
        backgroundColor:'#fff'
    }
}

export default getOption;