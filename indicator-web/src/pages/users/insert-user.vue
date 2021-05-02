<template>
  <q-page padding class="row justify-center items-start">
    <div class="col-10">
      <div style="height: 50px"></div>
      <insert-user-component />
    </div>
  </q-page>
</template>

<script>
import InsertUserComponent from "../../components/users/insert-user.vue";
export default {
  name: "InsertUser",
  components: {
    InsertUserComponent,
  },
};
</script>