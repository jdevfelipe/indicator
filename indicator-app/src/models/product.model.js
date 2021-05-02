export default class Product {
    constructor(data) {
        this.data = data
    }

    name() {
        return this.data.name
    }

    indicationPrice() {
        return this.data.indicationPrice
    }

    companyId() {
        return this.data.companyId
    }

    company() {
        return this.data.company
    }

    indications() {
        return this.data.indications
    }

    status() {
        return this.data.status
    }

}