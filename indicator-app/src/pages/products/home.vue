<template>
  <q-page class="flex column justify-start items-center q-pl-md q-pr-md q-pt-lg" padding>
    <div style="width: 100%;" class="q-mb-lg">
      <searchBar ref="searchBar" @getProductsBySearch="getProductsBySearch"/>
    </div>
    <div style="display: contents" v-if="products.length > 0">
      <div style="width: 100%;" class="q-mb-lg" v-for="product in productsArr">
        <productHomeCard :item="product" />
      </div>
    </div>
    <div class="q-pa-lg flex flex-center">
      <q-pagination
          v-model="current"
          color="primary"
          :max="maxPages"
          :max-pages="3"
          :ellipses="false"
          :boundary-numbers="false"
          @click="$refs.searchBar.text ? getProductsBySearch($refs.searchBar.text) : getAllProducts(current, 3)"
        >
        </q-pagination>
  </div>
  </q-page>
</template>

<script>
import searchBar from 'components/commons/search-bar'
import productHomeCard from 'components/products/home-card'
import ProductService from '../../services/product.service'
import Product from '../../models/product.model'
import NotifyService from '../../utils/notify.service'

const productService = new ProductService()
const notifyService = new NotifyService();

export default {
  name: 'productHome',
  components: {
    searchBar,
    productHomeCard
  },
  data() {
    return {
      products: [],
      current: 1,
      maxPages: 0
    }
  },
  methods: {
    getAllProducts(page, limit) {
      this.$q.loading.show()
      productService.getProducts(page, limit).then(res => {
        this.products = res.data.productDTOs
        this.maxPages = Math.ceil(res.data.total /3)
        this.$q.loading.hide()
      }).catch(e => {
        notifyService.notify(e.response.data.error, 'warning')
        this.$q.loading.hide()
      })
    },
    getProductsBySearch(text) {
      this.$q.loading.show()
      this.current = 1
      productService.getProductBySearch(text, this.current, 3).then(res => {
        debugger
        this.products = res.data.productDTOs
        this.maxPages = Math.ceil(res.data.total /3)
        this.$q.loading.hide()
      }).catch(e => {
        notifyService.notify(e.response.data.error, 'warning')
        this.$q.loading.hide()
      })

    }
  },
  created () {
    this.getAllProducts(1, 3)
  },
  computed: {
    productsArr () {
      return this.products
    }
  }
}
</script>
