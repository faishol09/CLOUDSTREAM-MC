// Cloudstream Addon JS untuk Ngefilm
// Versi minimal, fokus jalan dulu

const baseUrl = "https://new16.ngefilm.site"; // kalau domain berubah, ganti ini aja

export default new class Ngefilm extends ScriptedProvider {
    constructor() {
        super()
        this.id = "ngefilm"
        this.name = "NgefilmJS"
        this.language = "id"
        this.mainUrl = baseUrl
    }

    // Halaman utama (list film)
    async getMainPage(page) {
        const html = await this.request(`${baseUrl}/page/${page}/`, { method: "GET" })
        const $ = this.parseHTML(html)

        const results = []
        $("article").forEach(e => {
            const title = $(e).find("h2 a").text()
            const url = $(e).find("h2 a").attr("href")
            const poster = $(e).find("img").attr("src")
            if (title && url) {
                results.push({
                    url,
                    title,
                    poster
                })
            }
        })
        return results
    }

    // Detail film (judul, poster, link)
    async getDetail(url) {
        const html = await this.request(url, { method: "GET" })
        const $ = this.parseHTML(html)

        const title = $("h1").text()
        const poster = $("img").attr("src")
        const desc = $("p").first().text()

        const links = []
        $("iframe").forEach(e => {
            const src = $(e).attr("src")
            if (src) {
                links.push({
                    url: src,
                    isM3u8: src.includes(".m3u8")
                })
            }
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
