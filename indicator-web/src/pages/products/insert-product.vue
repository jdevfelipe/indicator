<template>
  <q-page padding class="row justify-center items-start">
    <div class="col-10">
      <div style="height: 50px"></div>
      <insert-product-component />
    </div>
  </q-page>
</template>

<script>
import InsertProductComponent from "../../components/products/insert-product.vue";
export default {
  name: "InsertProduct",
  components: {
    InsertProductComponent,
  },
};
</script>
