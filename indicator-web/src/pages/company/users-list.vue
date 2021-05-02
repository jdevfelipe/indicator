<template>
  <q-page padding class="row justify-center items-start">
    <div class="col-10">
      <div style="height: 50px"></div>
      <users-list-component v-if="usersComp && totalComp" @updateList="getUsersToList" :usersArray="usersComp" :total="totalComp" @delete="deleteUser"  />
    </div>
  </q-page>
</template>

<script>
import UsersListComponent from "../../components/company/users-list";
import UsersService from "../../services/users.service";
import CompaniesService from '../../services/companies.service'
import NotifyService from "../../utils/notify.service"

const usersService = new UsersService();
const notifyService = new NotifyService();
const companiesService = new CompaniesService();
export default {
  name: "UsersList",
  data () {
    return {
      usersArray: [],
      total: 0
    }
  },
  components: {
    UsersListComponent,
  },
  created() {
    this.getUsersToList(1, 4);
  },
  methods: {
    getUsersToList(page, rowsPerPage) {
      this.$q.loading.show();
      usersService.getUsers(page, rowsPerPage).then((res) => {
        this.$q.loading.hide();
        let vm = this
        vm.usersArray = res.data.userLoginDTOs
        vm.total = res.data.total
      }).catch(e => {
        this.$q.loading.hide();
        this.usersArray = []
        this.total = 0
        notifyService.notify(e.response.data.error, 'negative')
      });
    },
    deleteUser(id) {
      this.$q.loading.show();
      companiesService.deleteUser(id).then((res) => {
        this.$q.loading.hide();
        this.getUsersToList(1, 4)
      }).catch(e => {
        this.$q.loading.hide();
        notifyService.notify(e.response.data.error, 'negative')
      });
    }
  },
  computed: {
    usersComp () {
      console.log("aqui: " + JSON.stringify(this.usersArray));
      return this.usersArray
    },
    totalComp () {
      return this.total
    }
  }
};
</script>
