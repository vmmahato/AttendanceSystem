/**
 * Vuex
 *
 * @library
 *
 * https://vuex.vuejs.org/en/
 */

// Lib imports
import Vue from 'vue'
import Vuex from 'vuex'
// Store functionality
import actions from './actions'
import getters from './getters'
import mutations from './mutations'
import state from './state'

Vue.use(Vuex);
Vue.config.devtools = true

// Create a new store
const store = new Vuex.Store({
    actions,
    getters,
    mutations,
    state
})
export default store