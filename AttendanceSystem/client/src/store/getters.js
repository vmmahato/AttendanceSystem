export default {
    authenticated(state) {
        return state.token
    },
    getInfo(state) {
        return state.userInfo
    },
    getSystemUserData(state) {
        return JSON.parse(state.userInfo)
    },
    getTokenData(state) {
        return JSON.parse(state.tokenData)
    },
    checkDateLang(state) {
        return state.dateLang
    },
}