// Cloudstream Addon JS untuk Ngefilm
// Simpel: ambil list film, detail, dan link streaming

const baseUrl = "https://new16.ngefilm.site"; // kalau domain berubah, ganti ini aja

export default new class Ngefilm extends ScriptedProvider {
    constructor() {
        super()
        this.id = "ngefilm"
        this.name = "Ngefilm"
        this.language = "id"
        this.mainUrl = baseUrl
    }

    async getMainPage(page) {
        const html = await this.request(`${baseUrl}/page/${page}/`, { method: "GET" })
        const $ = this.parseHTML(html)

        const results = []
        $("article").forEach(e => {
            const title = $(e).find("h2 a").text()
            const url = $(e).find("h2 a").attr("href")
            const poster = $(e).find("img").attr("src")
            results.push({
                url,
                title,
                poster,
            })
        })
        return results
    }

    async getDetail(url) {
        const html = await this.request(url, { method: "GET" })
        const $ = this.parseHTML(html)

        const title = $("h1").text()
        const poster = $("img").attr("src")
        const desc = $("p").first().text()

        const links = []
        $("iframe").forEach(e => {
            links.push({
                url: $(e).attr("src"),
                isM3u8: false,
            })
        })

        return {
            title,
            poster,
            description: desc,
            episodes: [
                {
                    name: title,
                    links
                }
            ]
        }
    }
}
