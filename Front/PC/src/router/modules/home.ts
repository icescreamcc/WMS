import Layout from '@/layout/index.vue' 
import { createNameComponent } from '../createNode'
import BufferWarehouseDashboard from '@/views/production/bufferWarehouseDashboard/index.vue';
const route = [
  {
    path: '/',
    component: Layout,
    redirect: '/home',
    meta: { title: 'message.menu.dashboard.name', icon: 'iconfont ionfont-md icon-yingyongchengxu' },
    children: [
      {
        path: 'home',
        component: createNameComponent(() => import('@/views/home/index.vue')),
        meta: { title: 'message.menu.dashboard.index', icon: 'iconfont ionfont-md icon-yingyongchengxu', hideClose: true }
      }
    ]  
  } 
]

export default route