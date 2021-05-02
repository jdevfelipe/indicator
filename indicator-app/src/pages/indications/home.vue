<template>
  <q-page padding>
    <div class="flex justify-center" v-if="productNotFound">
      <span>{{messageError}}</span>
    </div>
    <div v-if="product && product.company.fantasyName">
      <q-expansion-item
          switch-toggle-side
          expand-separator
          icon="perm_identity"
          label="Detalhes do produto"
          :caption="product.name"
        >
          <q-card>
            <q-card-section>
              {{product.obs}}
            </q-card-section>
          </q-card>
      </q-expansion-item>
      <q-expansion-item
          switch-toggle-side
          expand-separator
          icon="perm_identity"
          label="Detalhes da empresa"
          :caption="product.company.fantasyName"
        >
          <q-card>
            <q-card-section>
              {{product.company.obs}}
            </q-card-section>
          </q-card>
      </q-expansion-item>
    </div>
    <q-separator/>
    <div class="row">
          <div class="col-12 q-pa-sm">
              <span class="text-primary">Cadastrar indicação:</span>
          </div>
          <div class="col-12 q-pa-sm">
            <div class="q-gutter-md">
              <q-select 
              outlined 
              v-model="indicationForm.documentType" 
              :options="DocumentTypeOptions"
              label="Tipo de documento"
              option-value="value"
              option-label="label"
              emit-value
              map-options />
            </div>
          </div>
          <div class="col-12 q-pa-sm">
            <div class="q-gutter-md">
              <q-input ref="agency" outlined v-model="indicationForm.document" label="Documento" type="text" />
            </div>
          </div>
          <div class="col-12 q-pa-sm">
            <div class="q-gutter-md">
              <q-input ref="account" outlined v-model="indicationForm.phone" label="Telefone" />
            </div>
          </div>
          <div class="col-12 q-pa-sm">
            <div class="q-gutter-md">
              <q-input ref="account" outlined v-model="indicationForm.cellPhone" label="Telefone 2" />
            </div>
          </div>
          <div class="col-12 q-pa-sm">
            <div class="q-gutter-md">
              <q-input ref="account" outlined v-model="indicationForm.email" label="email" />
            </div>
          </div>
          <div class="col-12 q-pa-sm">
            <div class="q-gutter-md">
              <q-select 
              outlined  
              v-model="indicationForm.branch" 
              :options="branchOptions" 
              label="Ramo"
              option-value="value"
              option-label="label"
              emit-value
              map-options />
            </div>
          </div>
          <div class="col-12 q-pa-sm">
            <div class="q-gutter-md">
              <q-input v-model="indicationForm.observation" outlined type="textarea" label="Obs:"/>
            </div>
          </div>
          <div class="col-12 flex justify-end q-pa-sm">
            <q-btn color="primary" label="Inserir" @click="insertNew"/>
          </div>
    </div>
    <DialogNotify 
    @closeNotify="isNotify = false, isSucces ? $router.push('/indications/my-indications') : $router.push('/products/home')"
     :model="isNotify"
     :title="titleRequest"
     :message="messageRequest"
     :icon="iconRequest"
     :color="colorRequest"/>
  </q-page>
</template>

<script>
import ProductService from '../../services/product.service'
import IndicationService from '../../services/indication.service'
import AuthService from '../../services/auth.service'
import NotifyService from '../../utils/notify.service'
import DialogNotify from '../../components/commons/dialog-notify'

const notifyService = new NotifyService()
const productService = new ProductService();
const indicationService = new IndicationService();
const authService = new AuthService();
export default {
  name: 'indication-home',
  components: {
    DialogNotify
  },
  data() {
    return {
      product: {},
      productNotFound: false,
      messageError: "",
      isSucces: false,
      isNotify: false,
      titleRequest: "",
      messageRequest: "",
      iconRequest: "",
      colorRequest: "",
      DocumentTypeOptions: [
        {
          label: "CNPJ",
          value: 0
        },
        {
          label: "CPF",
          value: 1
        }
      ],
      branchOptions: [
        {
          label: "Comércio",
          value: 0
        }
      ],
      indicationForm: {
        document: "",
        phone: "",
        cellPhone: "",
        email: "",
        observation: "",
        documentType: "",
        indicatorId: 0,
        productId: 0,
        branch: "",
        status: 0
      }
    }
  },
  created () {
    this.getProduct()
  },
  methods: {
    getProduct() {
      productService.getProduct(this.$route.params.productId).then(res => {
        debugger
        this.product = res.data
      }).catch(e => {
        if (e.response.status == 404) {
          notifyService.notify(e.response.data.error, 'warning')
        }
      })
    },
    insertNew() {
      this.$q.loading.show()
      this.indicationForm.productId = this.product.id
      let indicator = authService.getIndicatorLogged()
      this.indicationForm.indicatorId = indicator.id
      indicationService.doIndication(this.indicationForm).then(res => {
        this.$q.loading.hide()
        this.titleRequest = "Tudo certo!"
        this.messageRequest = "Agora aguarde a mudança de status de sua indicação, você também será notificado por E-mail!"
        this.iconRequest = "check_circle_outline"
        this.colorRequest = "bg-positive"
        this.isNotify = true
        this.isSucces = true
      }).catch(e => {
        this.$q.loading.hide()
        if (e.response.status == 400 && e.response.data.error) {
          notifyService.notify(e.response.data.error, 'negative')
          return
        }
        this.titleRequest = "Algo deu errado"
        this.messageRequest = "Parece que algo deu errado, tente outra hora ou entre em contato!"
        this.iconRequest = "error_outline"
        this.colorRequest = "bg-negative"
        this.isNotify = true
        this.isSucces = false
        console.log(e)
      })
      this.$q.loading.hide()
    }
  },
}
</script>
