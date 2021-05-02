import { http } from "src/boot/axios"

export default class IndicationService {
    doIndication = async (payload) => {
        let config = JSON.parse(localStorage.getItem('indicator'))
        const response = await http.post('indication/add-new/',  JSON.stringify(payload), { headers: { 'Authorization': `Bearer ${config.token}`}});
        return response
    }

    getIndications = async (text, page, limit) => {
        let config = JSON.parse(localStorage.getItem('indicator'))
        let indications = await http.get('indication/list-indications/' + config.id, { headers: { 'Authorization': `Bearer ${config.token}`, 'text': text, 'page': page, 'limit': limit}})
        return indications
    }

    setIndications = () => {

    }
}