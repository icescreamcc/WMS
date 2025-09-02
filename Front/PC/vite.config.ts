import { ConfigEnv, UserConfigExport } from 'vite'
import vue from '@vitejs/plugin-vue' 
import { resolve } from 'path'
import DefineOptions from 'unplugin-vue-define-options/vite';

const pathResolve = (dir: string): any => {
  return resolve(__dirname, ".", dir)
}

const alias: Record<string, string> = {
  '@': pathResolve("src")
}

// https://vitejs.dev/config/
export default ({ command }: ConfigEnv): UserConfigExport => { 
  return {
    css:{
            preprocessorOptions:{
              scss:{
                charset:false
              }
            }
          },
    resolve: {
      alias
    },
    server: {
      port: 3001,
      host: '0.0.0.0',
      open: true,
      proxy: {  
        '/dev': ''
      },
    },
    build: {
      rollupOptions: {
        output: {
          manualChunks: {
            'echarts': ['echarts']
          },
          assetFileNames: 'assets/[name].[ext]',
        }
      }, 
    }, 
    plugins: [
      vue(),
      DefineOptions() 
    ]  
  };
}
