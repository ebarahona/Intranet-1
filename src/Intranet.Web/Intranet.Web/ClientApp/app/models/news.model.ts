import { Image, User} from './'
import { HasId } from '../contracts'

export class News implements HasId {
    id: number | null
    title: string
    text: string
    created: Date
    updated: Date
    headerImage: Image
    user: User
    keywords: string
    published: boolean
    hasEverBeenPublished: boolean
    url: string
}
