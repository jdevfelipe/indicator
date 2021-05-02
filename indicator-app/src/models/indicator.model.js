export default class Indicator {
    constructor(data) {
        this.data = data
    }

    name() {
        return this.data.name
    }

    email() {
        return this.data.email
    }

    CPF() {
        return this.data.CPF
    }

    token() {
        return this.data.token
    }

    indications() {
        return this.data.indications
    }

    confirmCode() {
        return this.data.confirmCode
    }

    isConfirmed() 
    {
        return this.data.isConfirmed
    }

    paymentAccounts() {
        return this.data.paymentAccounts
    }
    
    status() {
        return this.data.status
    }
}