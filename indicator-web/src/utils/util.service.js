export default class UtilService {
    convertDate  (dateTime)  {
        let date = dateTime.split('T')[0]
        let day = date.split('-')[2]
        let month = date.split('-')[1]
        let year = date.split('-')[0]
  
        let hour = dateTime.split('T')[1]
        let finalHour = hour.split('.')[0]
  
        return `${day}/${month}/${year} - ${finalHour}`
    } 
}