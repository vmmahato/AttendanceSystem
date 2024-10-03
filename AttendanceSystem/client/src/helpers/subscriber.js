import store from '@/store'
import axios from 'axios'
import router from "../router";

store.subscribe((mutation) => {

    //comes when mutation state.token in auth.js

    switch (mutation.type) {
        case 'setToken': {
            //mutation.payload contain token
            console.log('set token mutation subscriber')
            if (mutation.payload) {
                console.log('set token mutation subscriber mutation payload')
                axios.defaults.headers.common['Authorization'] = `Bearer ${mutation.payload}`
                //token store in local storage
                localStorage.setItem('token', mutation.payload)
                if (localStorage.getItem('token')) {
                    var base64Url = localStorage.getItem('token').split('.')[1];
                    var base64 = base64Url.replace(/-/g, '+').replace(/_/g, '/');
                    var jsonPayload = decodeURIComponent(atob(base64).split('').map(function (c) {
                        return '%' + ('00' + c.charCodeAt(0).toString(16)).slice(-2);
                    }).join(''));
                    localStorage.setItem('tokenData', jsonPayload)
                }

                //redirect after successful login
            }
            break
        }
        case 'setUserInfo': {
            console.log('set setUserInfo subscriber')
            if (mutation.payload) {
                if (localStorage.getItem('userInfo')) {
                    break
                } else {
                    localStorage.setItem('userInfo', JSON.stringify(mutation.payload))
                }
            }
            // let redirectData = JSON.parse(localStorage.getItem('tokenData'))
            let userInfo = JSON.parse(localStorage.getItem('userInfo'))
            console.log('userInfo here', userInfo)
            router.push(`${userInfo.dashBoard}`)

            break
        }
        case 'setRefreshToken': {
            if (mutation.payload) {
                if (localStorage.getItem('refreshToken')) {
                    break
                } else {
                    localStorage.setItem('refreshToken', mutation.payload)
                }
            }
            break
        }
        case 'SET_DATE_LANG': {
            if (localStorage.getItem('dateLang')) {
                if (JSON.parse(localStorage.getItem('dateLang')) === mutation.payload) {
                    break
                } else{
                    localStorage.setItem('dateLang', mutation.payload)
                    break
                }
            } else {
                localStorage.setItem('dateLang', mutation.payload)
                break
            }
        }
    }
})