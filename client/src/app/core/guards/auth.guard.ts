import { inject } from "@angular/core";
import { CanActivateChildFn, Router } from "@angular/router";
import { AuthService } from "../services/auth.service";

export const authGuard: CanActivateChildFn = async () => {
  const auth = inject(AuthService);
  const router = inject(Router);

  const user = await auth.loadUser();

  return user
    ? true
    : router.createUrlTree(['/login']);
};
