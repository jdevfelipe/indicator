<template>
  <q-page padding class="row justify-center items-start">
    <div class="col-10">
      <div style="height: 50px"></div>
      <register-manager-component />
    </div>
  </q-page>
</template>

<script>
import RegisterManagerComponent from "../../components/auth/registerManager";
export default {
  name: "RegisterManager",
  components: {
    RegisterManagerComponent,
  },
};
</script>
