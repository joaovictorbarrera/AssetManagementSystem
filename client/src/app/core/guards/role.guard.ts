import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { AuthService } from '../services/auth.service';
import { Role } from '../enums/role';

export const roleGuard: CanActivateFn = (route) => {
    const authService = inject(AuthService)
    const router = inject(Router)

    const allowedRoles = route.data?.['roles'] as Role[];
    const userRole = authService.currentUser()?.role;

    const authorized = allowedRoles.some(
        role => role === userRole
    )

    return authorized ? authorized : router.createUrlTree(['/unauthorized'])
}
