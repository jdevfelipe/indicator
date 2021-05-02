import { http } from "src/boot/axios"

export default class DashService {

    getIndicationsByProduct = async (payload, date) => {
        let config = JSON.parse(localStorage.getItem('user'))
        const response = await http.get('indication/list-indications-by-product/', { headers: { 'Authorization': `Bearer ${config.token}`, 'companies': payload, 'initialDate': date.initialDate, 'finishDate': date.finishDate}});
        return response
    }
}