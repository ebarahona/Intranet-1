import { Directive, Input } from '@angular/core';

// RouterLink
@Directive({
    selector: '[routerLink]',
    host: {
        '(click)': 'onClick()'
    }
})
export class RouterLinkStubDirective {
    @Input('routerLink') linkParams: any;
    navigatedTo: any = null;

    onClick() {
        this.navigatedTo = this.linkParams;
    }
}

// RouterLinkActive
@Directive({
    selector: '[routerLinkActive]',
})
export class RouterLinkActiveStubDiretive {
    @Input('routerLinkActive') listParams: any;
}