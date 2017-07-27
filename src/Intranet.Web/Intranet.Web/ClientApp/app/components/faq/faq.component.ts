import { Component, OnInit } from '@angular/core'
import * as _ from 'lodash'
import { Observable } from 'rxjs/Observable'
import {
  Category,
  Faq,
  FaqByCategory
} from '../../models'
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
  saved: { faqId: number, success: string | null, error: string | null }

  constructor(
    private authenticationService: AuthenticationService,
    private faqService: FaqService
  ) {
    this.openFaqs = []
    this.saved = {
      faqId: null,
      success: null,
      error: null,
    }
  }

  async ngOnInit() {
    this.updateData()
    this.isAdmin = await this.authenticationService.isAdmin()
  }

  onCategoryTitleChange(content: string, category: Category) {
    console.log(content)
  }

  onQuestionChange(content: string, faq: Faq) {
    faq.question = content
  }

  onAnswerChange(content: string, faq: Faq) {
    faq.answer = content
  }

  onKeywordsChange(content: string, faq: Faq) {
    faq.keywords = content
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
          if (this.isAdmin) {
            for (const faqByCategory of faqByCategories) {
              const newFaq = new Faq()
              newFaq.category = new Category()
              newFaq.answer = 'Enter a new answer.'
              newFaq.question = 'Enter a new question.'
              newFaq.keywords = 'Add Keywords!'
              newFaq.category.title = faqByCategory.category.title

              faqByCategory.faqs.push(newFaq)
            }
          }
          this.faqByCategories = faqByCategories
        }
      )
  }

  async save(faq: Faq) {
    faq.category.faqs = null
    faq.faqKeywords = null

    let obs: Observable<Faq>

    if (faq.id) {
      obs = await this.faqService.putItem(faq)
    } else {
      obs = await this.faqService.postItem(faq)
    }

    obs.subscribe(
      (data) => {
        faq.id = data.id
        this.saved.faqId = data.id
        this.saved.success = 'Updated successfully!'
        this.saved.error = null
        this.updateData()
      },
      error => {
        this.saved.faqId = faq.id
        this.saved.success = null
        this.saved.error = error
      }
    )
  }
}
