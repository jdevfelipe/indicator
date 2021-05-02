import { http } from "src/boot/axios"

export default class AuthService {

    indicatorLogin = async (payload) => {
        const response = await http.post('auth/indicator/login/', JSON.stringify(payload));
        return response
    }

    login = (payload) => {
        localStorage.setItem('indicator', JSON.stringify(payload))
    }
    
    logout = () => {
        localStorage.clear('indicator')
        let indicator = localStorage.getItem('indicator')
        return indicator
    }

    indicatorIsLogged = () => {
        let indicator = JSON.parse(localStorage.getItem('indicator'))
        if (indicator && indicator.token) {
            return true
        }
        return false
    }

    getIndicatorLogged = () => {
        let indicator = JSON.parse(localStorage.getItem('indicator'))
        return indicator
    }

    registerNewIndicator = async (payload) => {
        const response = await http.post('auth/indicator/register/', JSON.stringify(payload));
        return response
    }

    verifyIfCodeIsOK = async (payload, code) => {
        const response = await http.post('auth/indicator/verify-code/' + code, JSON.stringify(payload));
        return response
    }

    generateNewCode = async (payload) => {
        const response = await http.post('auth/indicator/generate-new-code/', JSON.stringify(payload));
        return response
    }

    verifyIfTokenIsValid= async () => {
        let indicator = JSON.parse(localStorage.getItem('indicator'))
        const response = await http.post('auth/indicator/validate-token/', JSON.stringify(indicator));
        return response
    }

    getIndicatorLoggedFromUpdate = async () => {
        let config = JSON.parse(localStorage.getItem('indicator'))
        const res = await http.get('indicator/update-profile/', {headers: {'Authorization': `Bearer ${config.token}`, 'id': config.id}})
        localStorage.setItem('indicator', JSON.stringify(res.data))
        location.reload();
    }

    updateProfile = async (payload) => {
        let config = JSON.parse(localStorage.getItem('indicator'))
        const res = await http.post('indicator/update/', JSON.stringify(payload), {headers: {'Authorization': `Bearer ${config.token}`}})
        return res;
    }

    updateBank = async (payload) => {
        let config = JSON.parse(localStorage.getItem('indicator'))
        const res = await http.post('indicator/bank/update/', JSON.stringify(payload), {headers: {'Authorization': `Bearer ${config.token}`}})
        return res;
    }

    forgotPassRequest = async (payload) => {
        const res = await http.get('indicator/forgot-pass/', {headers: {'document': payload}})

        return res;
    }

}