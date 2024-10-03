import Vue from 'vue'
import VueRouter from 'vue-router'
import paths from './paths'
import store from '@/store'

Vue.use(VueRouter)

const router = new VueRouter({
  mode: 'history',
  base: process.env.BASE_URL,
  routes:paths
})

router.beforeEach((to, from, next) => {
  if (to.matched.some(route => route.meta.requiresAuth)) {
    if (!store.getters['authenticated']) {
      return next('/systemLogin')
    }
    else {
      // console.log(`from name ${from.name}`)
      // if(`${from.name}` == "userlogin")
      next()
    }
  } else {
    //next()
    if(to.name==='home' && store.getters['authenticated']){
      next('/dashboard')
    }
    else{
      next()
    }
  }
  if (to.meta.conditionalAuth) {
    if (store.getters['authenticated']) {
      next()
    }
    else {
      // console.log(`from name ${from.name}`)
      // if(`${from.name}` == "userlogin")

      return next('/systemLogin')
    }
  } else {
    next()
  }
})

export default router
