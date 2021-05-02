// import something here
import AuthService from 'app/src/services/auth.service'
const authService = new AuthService()

export default async ({ router }) => {
  router.beforeEach((to, from, next) => {
      if (to.path !== '/auth' && !authService.indicatorIsLogged()) {
        next({ path: '/auth' })
      } else {
        next()
      }
  })
}
