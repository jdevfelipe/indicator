import { http } from "src/boot/axios"

export default class ProductsService {

    insertProduct = async (payload) => {
        let config = JSON.parse(localStorage.getItem('user'))
        const response = await http.post('product/add-new/', JSON.stringify(payload), { headers: { 'Authorization': `Bearer ${config.token}`}});
        return response
    }

    getProductsByCompanies = async (payload, page, rowsPerPage) => {
        let config = JSON.parse(localStorage.getItem('user'))
        const response = await http.get('product/get-products-by-company/', { headers: { 'Authorization': `Bearer ${config.token}`, 'companyIds': payload, 'page': page, 'limit': rowsPerPage}});
        return response
    }
}