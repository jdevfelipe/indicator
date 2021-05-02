import { http } from "src/boot/axios"

export default class IndicationService {
    getPayments = async (text, page, limit) => {
        let config = JSON.parse(localStorage.getItem('indicator'))
        const response = await http.get('payment/list-payments/' +  config.id, { headers: { 'Authorization': `Bearer ${config.token}`, 'searchTerm': text, 'page': page, 'limit': limit}});
        return response
    }

    confirmReceive = async (payload) => {
        let config = JSON.parse(localStorage.getItem('indicator'))
        const response = await http.get('payment/confirm-payment-receive', { headers: { 'Authorization': `Bearer ${config.token}`, 'paymentId': payload}});
        return response
    }

    showPayment = async (payload) =>{
        let config = JSON.parse(localStorage.getItem('indicator'))
        const response = await http.get('payment/get-pay-paper', { headers: { 'Authorization': `Bearer ${config.token}`, 'paymentId': payload}});
        return response
    }
}