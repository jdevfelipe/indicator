<template>
  <q-page padding class="row justify-center items-start">
    <div class="col-10">
      <div style="height: 50px"></div>
      <list-users-component v-if="users && total"  :usersArray="users" :total="total" @updateList="getUsersToListByCompany"/>
    </div>
  </q-page>
</template>

<script>
import ListUsersComponent from "../../components/users/list-users";
import UsersService from "../../services/users.service";
import NotifyService from "../../utils/notify.service";
import AuthService from "../../services/auth.service";

const usersService = new UsersService();
const authService = new AuthService();
const notifyService = new NotifyService();
export default {
  name: "ListUsers",
  data () {
    return {
      usersArray: [],
      companiesIds: [],
      total: 0
    }
  },
  components: {
    ListUsersComponent,
  },
  created() {
    this.createCompanyList();
    this.getUsersToListByCompany(1, 2);
  },
  methods: {
    createCompanyList() {
      for (let c of this.user.userCompanies) {
        this.companiesIds.push(c.companyId);
      }
    },
    getUsersToListByCompany(page, rowsPerPage) {
      this.$q.loading.show();
      usersService.getUsersByCompany(this.companiesIds, page, rowsPerPage).then((res) => {
        this.$q.loading.hide();
        let vm = this
        vm.usersArray = res.data.users;
        vm.total = res.data.total
      }).catch(e => {
        this.$q.loading.hide();
        this.usersArray = []
        this.total = 0
      });
    },
  },
  computed: {
    users () {
      return this.usersArray
    },
    user () {
      return authService.getUserLogged();
    }
  }
};
</script>

