import { http } from "src/boot/axios"

export default class CompaniesService {

    getCompanies = async (page, limit) => {
        let config = JSON.parse(localStorage.getItem('user'))
        const response = await http.get('company/get-companies/', { headers: { 'Authorization': `Bearer ${config.token}`, 'page': page, 'limit': limit }});
        return response
    }

    getCompaniesById = async (payload) => {
        let config = JSON.parse(localStorage.getItem('user'))
        const response = await http.get('company/get-companies-by-id/', { headers: { 'Authorization': `Bearer ${config.token}`, 'companiesIds': payload}});
        return response
    }

    updateUser = async (user) => {
        let config = JSON.parse(localStorage.getItem('user'))
        const response = await http.post('company/update/', JSON.stringify(user), { headers: { 'Authorization': `Bearer ${config.token}`}});
        return response
    }

    deleteUser = async (id) => {
        let config = JSON.parse(localStorage.getItem('user'))
        const response = await http.delete('company/delete/', { headers: { 'Authorization': `Bearer ${config.token}`, 'id': id}});
        return response
    }

    updateCompany = async (company) => {
        let config = JSON.parse(localStorage.getItem('user'))
        const response = await http.post('company/update/company/', JSON.stringify(company), { headers: { 'Authorization': `Bearer ${config.token}`}});
        return response
    }
}