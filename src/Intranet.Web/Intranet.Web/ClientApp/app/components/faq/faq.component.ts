import { Component, OnInit } from '@angular/core'
import * as _ from 'lodash'
import { Faq, FaqByCategory } from '../../models'
import { AuthenticationService, FaqService } from '../../_services'

// See https://github.com/lodash/lodash/issues/1677#issuecomment-306119559
const toggler = (collection, item) => {
    const idx = _.indexOf(collection, item)
    if (idx !== -1) {
        collection.splice(idx, 1)
    } else {
        collection.push(item)
    }
}

@Component({
  selector: 'faq',
  templateUrl: './faq.component.html',
  styleUrls: ['./faq.component.css']

})
export class FaqComponent implements OnInit {
  faqByCategories: FaqByCategory[]
  openFaqs: number[]
  isAdmin: boolean

  constructor(
    private authenticationService: AuthenticationService,
    private faqService: FaqService
  ) {
    this.openFaqs = []
  }

  async ngOnInit() {
    this.updateData()
    this.isAdmin = await this.authenticationService.isAdmin()
  }

  onEditorContentChange(content: string) {
    console.log(content)
  }

  handleOnDelete(info: string) {
    this.updateData()
  }

  toggle(id: number) {
    _.partial(toggler, this.openFaqs)(id)
  }

  isOpen(id: number) {
    return _.includes(this.openFaqs, id)
  }

  updateData() {
    this.faqService.getFaqsByCategory().subscribe(
        faqByCategories => {
          this.faqByCategories = faqByCategories
        }
      )
  }
}
