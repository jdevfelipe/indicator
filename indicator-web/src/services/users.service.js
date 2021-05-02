import { http } from "src/boot/axios"

export default class UsersService {

    getUsers = async (page, rowsPerPage) => {
        let config = JSON.parse(localStorage.getItem('user'))
        const response = await http.get('user/get-users/', { headers: { 'Authorization': `Bearer ${config.token}`, 'page': page, 'limit': rowsPerPage}});
        return response
    }

    insertUser = async (payload, companyIds) => {
        let config = JSON.parse(localStorage.getItem('user'))
        const response = await http.post('user/insert-users/', JSON.stringify(payload), { headers: { 'Authorization': `Bearer ${config.token}`, 'companiesId': companyIds}});
        return response
    }

    getUsersByCompany = async (payload, page, rowsPerPage) => {
        let config = JSON.parse(localStorage.getItem('user'))
        const response = await http.get('user/list-users-by-companies/', { headers: { 'Authorization': `Bearer ${config.token}`, 'companies': payload, 'page': page, 'limit': rowsPerPage}});
        return response
    }
}