<template>
  <q-page padding class="row justify-center items-start">
    <div class="col-10">
      <div style="height: 50px"></div>
      <products-list-component v-if="products && totalComp" :productsArray="products" :total="total" @updateList="getProductsToList" />
    </div>
  </q-page>
</template>

<script>
import ProductsListComponent from "../../components/products/list-products";
import AuthService from "../../services/auth.service";
import ProductsService from "../../services/products.service";
import NotifyService from "../../utils/notify.service"

const productsService = new ProductsService();
const authService = new AuthService();
const notifyService = new NotifyService();
export default {
  name: "ProductsList",
  data () {
    return {
      productsArray: [],
      companiesIds: [],
      total: 0
    }
  },
  components: {
    ProductsListComponent,
  },
  created() {
    this.createCompanyList();
    this.getProductsToList(1, 1);
  },
  methods: {
    createCompanyList() {
      for (let c of this.user.userCompanies) {
        this.companiesIds.push(c.companyId);
      }
    },
    getProductsToList(page, rowsPerPage) {
      this.$q.loading.show();
      productsService.getProductsByCompanies(this.companiesIds, page, rowsPerPage).then((res) => {
        this.$q.loading.hide();
        let vm = this
        vm.productsArray = res.data.products
        vm.total = res.data.total
      }).catch(e => {
        this.$q.loading.hide();
        this.productsArray = []
      });
    },
  },
  computed: {
    products () {
      return this.productsArray
    },
    user () {
      return authService.getUserLogged();
    },
    totalComp () {
      return this.total
    }
  }
};
</script>