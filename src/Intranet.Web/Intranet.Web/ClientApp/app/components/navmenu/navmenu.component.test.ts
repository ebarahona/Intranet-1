import { NavMenuComponent } from './navmenu.component'
import { TestBed, ComponentFixture } from '@angular/core/testing'
import { RouterLinkStubDirective, RouterLinkActiveStubDiretive } from '../../../../Tests/routerStub'

let fixture: ComponentFixture<NavMenuComponent>
let component: NavMenuComponent

describe('NavMenu component', () => {
    beforeEach(() => {
        TestBed.configureTestingModule({
            declarations: [
                NavMenuComponent,
                RouterLinkStubDirective,
                RouterLinkActiveStubDiretive
            ]
        })
            .compileComponents()
            .then(() => {
                fixture = TestBed.createComponent(NavMenuComponent)
                component = fixture.componentInstance
                fixture.detectChanges()
            })
    })

    it('should have an "IsIn" variable that starts as "false"', () => {
        const IsIn = component.isIn
        expect(IsIn).toBe(false)
    })

    it('should toggle the value of IsIn when toggleState() is called', () => {
        const startValue = component.isIn
        component.toggleState()
        const toggledValue_1 = component.isIn
        expect(toggledValue_1).not.toBe(startValue)

        component.toggleState()
        const toggledValue_2 = component.isIn
        expect(toggledValue_2).toBe(startValue)
    })
})