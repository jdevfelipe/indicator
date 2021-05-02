import { http } from "src/boot/axios"

export default class IndicationsService {

    changeStatus = async (indicationId, status) => {
        let config = JSON.parse(localStorage.getItem('user'))
        const response = await http.get('indication/change-indication-status/' + indicationId, { headers: { 'Authorization': `Bearer ${config.token}`, 'status': status}});
        return response
    }
}