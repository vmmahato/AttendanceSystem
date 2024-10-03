import axios from 'axios'

export default {
    //after signin clicked the process comes in sign in
    async signIn({ dispatch }, credential) {

        const headers = { 'Content-Type': 'application/json' }

        try {
            //api called for credential check and getting token
            let response = await axios.post('Account/Login', credential, { headers: headers })
            //called attempt method and send token from the response
            console.log('response.data.data',response.data.data)
            dispatch('attempt', response.data.token);
            dispatch('userData', response.data.data);
            dispatch('refreshToken', response.data.data.refreshToken);
            dispatch('setAllData', response.data.data);
        }
        catch (e) {
            return (e.response);
        }
    },
    async attempt({ commit, state }, token) {

        if (token) {
            //called set token method to save token in local storage
            commit('setToken', token)
            commit('setTokenData', token)

        }
        if (!state.token) {
            localStorage.removeItem('token')
            localStorage.removeItem('userInfo')
            localStorage.removeItem('tokenData')
            delete axios.defaults.headers.common['Authorization']
            return
        }
    },
    async userData({ commit, state }, userInfo) {
        if (userInfo) {
            commit('setUserInfo', userInfo)
        }
        if (!state.userInfo) {
            return
        }
    },
    async refreshToken({ commit, state }, refreshToken) {
        if (refreshToken) {
            commit('setRefreshToken', refreshToken)
        }
        if (!state.refreshToken) {
            return
        }
    },
    async setAllData({ commit, state }, data) {
        if (data) {
            commit('setAllData', data)
        }
        if (!state.refreshToken) {
            return
        }
    },
    logout({ commit }) {
        return new Promise((resolve) => {
            axios.post('Account/Logout').then(response => {
                if (response) {
                    localStorage.removeItem('token')
                    localStorage.removeItem('userInfo')
                    localStorage.removeItem('tokenData')
                    delete axios.defaults.headers.common['Authorization']

                }
            }).catch(e => {
                console.log(e)
            })
            commit('logout')
            resolve()
        })
    },

    set_lang({commit}, lang){
        commit('SET_DATE_LANG',lang)
    }
}