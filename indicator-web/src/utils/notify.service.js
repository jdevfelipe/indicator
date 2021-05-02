import { Notify } from 'quasar'
export default class NotifyService {
    notify (message, type) {
        Notify.create({
            type: type,
            message: message
        })
    }
}