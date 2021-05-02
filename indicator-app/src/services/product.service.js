import { http } from "src/boot/axios"

export default class ProductService {
    getProducts = async (page, limit) => {
        let config = JSON.parse(localStorage.getItem('indicator'))
        let products = await http.get('product/get-products/', { headers: { 'Authorization': `Bearer ${config.token}`, 'page': page, 'limit': limit}})
        return products
    }

    getProduct = async (productId) => {
        let config = JSON.parse(localStorage.getItem('indicator'))
        let product = await http.get('product/get-product/' + productId, { headers: { 'Authorization': `Bearer ${config.token}`}})
        return product
    }

    getProductBySearch = async (term, page, limit) => {
        let config = JSON.parse(localStorage.getItem('indicator'))
        let product = await http.get('product/search/', { headers: { 'Authorization': `Bearer ${config.token}`, 'searchTerm': term, 'page': page, 'limit': limit}})
        return product
    }
}