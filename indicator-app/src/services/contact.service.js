import { http } from "src/boot/axios"

export default class ContactService {
    sendMessage = async (payload) => {
        let config = JSON.parse(localStorage.getItem('indicator'))
        const res = await http.post('indicator/sendmail/send-new/', JSON.stringify(payload), {headers: {'Authorization': `Bearer ${config.token}`}})
        return res;
    }
}