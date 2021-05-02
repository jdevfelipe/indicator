import { http } from "src/boot/axios"

export default class AuthService {

    userIsLogged = () => {
        let user = JSON.parse(localStorage.getItem('user'))
        if (user && user.token) {
            return true
        }
        return false
    }

    userLogin = async (payload) => {
        const response = await http.post('auth/company/login/', JSON.stringify(payload));
        return response
    }

    login = (payload) => {
        localStorage.setItem('user', JSON.stringify(payload))
    }
    
    logout = () => {
        localStorage.clear('user')
        let indicator = localStorage.getItem('user')
        return indicator
    }

    getUserLogged = () => {
        let user = JSON.parse(localStorage.getItem('user'))
        return user
    }

    userRegister = async (payload) => {
        let config = JSON.parse(localStorage.getItem('user'))
        const response = await http.post('auth/company/register/', JSON.stringify(payload), { headers: { 'Authorization': `Bearer ${config.token}`}});
        return response
    }

    companyRegister = async (payload) => {
        let config = JSON.parse(localStorage.getItem('user'))
        const response = await http.post('company/add-new/', JSON.stringify(payload), { headers: { 'Authorization': `Bearer ${config.token}`}});
        return response
    }

    verifyIfTokenIsValid= async () => {
        let config = JSON.parse(localStorage.getItem('user'))
        const response = await http.post('auth/indicator/validate-token/', JSON.stringify(config));
        return response
    }

    forgotPassRequest = async (payload) => {
        const response = await http.get('company/forgot-pass/', { headers: {'document': payload}});
        return response
    }

}