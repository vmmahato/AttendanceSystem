import Vue from 'vue'
import App from './App.vue'
import vuetify from './plugins/vuetify'
import router from './router'
import Vuex from 'vuex'
import axios from 'axios'
import store from './store'
import Validate from 'vuelidate'
import './static/globalStyle.css'
import VDigitalTimePicker from 'v-digital-time-picker'


require('@/helpers/subscriber')

Vue.config.productionTip = false

Vue.mixin({
  data(){
    return{
      tokenData: JSON.parse(localStorage.getItem("tokenData")),
      userData: JSON.parse(localStorage.getItem("userInfo"))
    }
  },
})
axios.defaults.baseURL = 'https://localhost:44362/api/';
axios.defaults.headers.common['Content-Type'] = 'application/json';

Vue.use(vuetify);
Vue.use(Vuex)
Vue.use(require('vue-moment'));
Vue.use(Validate)
Vue.use(VDigitalTimePicker)

axios.interceptors.response.use(
    response => {
      return response;
    },
    err => {
      const {
        response: { status , data },
      } = err;
      if (status === 401 || data.Message === 'Invalid Access Token') {
        router.push({ name: "home" });
        localStorage.clear()
        store.commit('logout')
        delete axios.defaults.headers.common['Authorization']
        return Promise.reject(false);
      }
      /*if (status === 400 && data.State === "AccessTokenExpired") {
        localStorage.removeItem('token')
      }*/
    })

store.dispatch('attempt',localStorage.getItem('token')).then( ()=>{
  new Vue({
    vuetify,
    router,
    store,
    render: h => h(App)
  }).$mount('#app')
})

