// import something here
import AuthService from 'app/src/services/auth.service'
const authService = new AuthService()
const analystRoutes = ['/auth/register-manager', '/company/users-list', '/company/companies-list']
const managerRoutes = ['/home', '/products/add-new', '/products/list', '/users/insert-user', '/users/list-user', '/company/list-by-manager']
const userRoutes = ['/home']
let user = JSON.parse(localStorage.getItem('user'))

export default async ({ router }) => {
  router.beforeEach((to, from, next) => {
    // if (!analystRoutes.includes(to.path) && user.userRole != 2) {
    //   console.log('not auth to this route')
    //   next({path: '/home'})
    // } else if (!managerRoutes.includes(to.path) && user.userRole != 1) {
    //   console.log('not auth to this route')
    //   next({path: '/auth/register-manager'})
    // } else if (!userRoutes.includes(to.path) && user.userRole != 0) {
    //   console.log('not auth to this route')
    //   next({path: '/home'})
    // }
    if (to.path !== '/auth/login' && to.path !== '/auth/register' && !authService.userIsLogged()) {
      next({ path: '/auth/login' })
    } else {
      next()
    }
  })
}
