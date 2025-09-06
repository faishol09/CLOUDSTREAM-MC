const baseUrl = "https://new16.ngefilm.site"

export default new class NgefilmDebug extends ScriptedProvider {
    constructor() {
        super()
        this.id = "ngefilmdebug"
        this.name = "NgefilmJS (Debug)"
        this.language = "id"
        this.mainUrl = baseUrl
    }

    async getMainPage(page) {
        try {
            const html = await this.request(`${baseUrl}/page/${page}/`, { method: "GET" })
            console.log("DEBUG getMainPage HTML length:", html.length)

            const $ = this.parseHTML(html)
            const results = []

            $("article").forEach(e => {
                const title = $(e).find("h2 a").text()
                const url = $(e).find("h2 a").attr("href")
                const poster = $(e).find("img").attr("src")
                console.log("DEBUG movie:", title, url, poster)

                if (title && url) {
                    results.push({ url, title, poster })
                }
            })

            console.log("DEBUG total results:", results.length)
            return results
        } catch (err) {
            console.log("ERROR getMainPage:", err)
            return []
        }
    }

    async getDetail(url) {
        try {
            const html = await this.request(url, { method: "GET" })
            console.log("DEBUG getDetail URL:", url, "HTML length:", html.length)

            const $ = this.parseHTML(html)
            const title = $("h1").text()
            const poster = $("img").attr("src")
            const desc = $("p").first().text()

            const links = []
            $("iframe").forEach(e => {
                const src = $(e).attr("src")
                if (src) {
                    console.log("DEBUG iframe link:", src)
