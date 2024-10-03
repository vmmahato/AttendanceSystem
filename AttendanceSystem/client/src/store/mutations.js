export default {
    setToken(state, token) {
        //putting the value of token in state and directed to subscriber.js
        state.token = token
        /*if (localStorage.getItem('token')) {
            var base64Url = localStorage.getItem('token').split('.')[1];
            var base64 = base64Url.replace(/-/g, '+').replace(/_/g, '/');
            var jsonPayload = decodeURIComponent(atob(base64).split('').map(function (c) {
                return '%' + ('00' + c.charCodeAt(0).toString(16)).slice(-2);
            }).join(''));
        }
        state.tokenData = jsonPayload*/

    },
    setTokenData(state) {
        //putting the value of token in state and directed to subscriber.js
        if (localStorage.getItem('token')) {
            var base64Url = localStorage.getItem('token').split('.')[1];
            var base64 = base64Url.replace(/-/g, '+').replace(/_/g, '/');
            var jsonPayload = decodeURIComponent(atob(base64).split('').map(function (c) {
                return '%' + ('00' + c.charCodeAt(0).toString(16)).slice(-2);
            }).join(''));
        }
        state.tokenData = jsonPayload
        console.log(state.tokenData)

    },
    setUserInfo(state, userInfo) {
        state.userInfo = userInfo
        // state.refreshToken = JSON.parse(userInfo).refreshToken
    },
    setRefreshToken(state, refreshToken) {
        state.refreshToken = refreshToken
    },

    setAllData(state, data) {
        state.allData = data
        console.log('setAllData', state.allData)
    },

    logout(state) {
        state.token = null
        state.userInfo = null
        state.tokenData = null
        state.refreshToken = null

    },
    SET_DATE_LANG(state, lang){
        state.dateLang = lang
    }
}