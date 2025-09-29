  
const getOption=(title:string, year:any,maxValue:number, monthArray:Array<number>,IndicatorArray:Array<any>,dataArray:Array<any>)=>{
   return {
        title: {
            text: `${title}消耗趋势`,
            subtext: year,
            top: -4,
            left: 1,
            textStyle:{ fontSize:15}, 
        },
        tooltip: {
            trigger: 'item',
            position: ['50%', '50%']
        },
        legend: {
            type: 'scroll',
            bottom: -5,
            padding: [50, 5, 5, 5],
            data: monthArray
        },
        visualMap: {
            top: 'middle',
            right: '3%',
            color: ['red', 'yellow'],
            calculable: false, 
            // max:maxValue, 
             itemWidth: 10,
             text:['H', 'L'],
             textStyle:{ color:'#a7a6a6' }, 
        },
        radar: {
            indicator: IndicatorArray, 
            radius: '65%'
        },
        series: (function (){
            var series = [];
            for (var i = 1; i <= dataArray.length; i++) {
                series.push({
                    name: title,
                    type: 'radar',
                    symbol: 'none',
                    lineStyle: {
                        width: 1
                    },
                    emphasis: {
                        areaStyle: {
                            color: 'rgba(0,250,0,0.3)'
                        }
                    },
                    data:dataArray
                });
            }
            return series;
        })()
      };
}

   
  export default getOption