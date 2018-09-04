
import scala.concurrent.duration._

import io.gatling.core.Predef._
import io.gatling.http.Predef._
import io.gatling.jdbc.Predef._

class MedlearnTest2StudentLogin extends Simulation {

	val httpProtocol = http
		.baseURL("https://medlearn-test2.medicine.arizona.edu")
		.inferHtmlResources()
		.acceptHeader("*/*")
		.acceptEncodingHeader("gzip, deflate")
		.acceptLanguageHeader("en-US,en;q=0.5")
		.userAgentHeader("Mozilla/5.0 (Windows NT 10.0; WOW64; rv:60.0) Gecko/20100101 Firefox/60.0")

	val headers_0 = Map(
		"Accept-Encoding" -> "gzip, deflate",
		"Pragma" -> "no-cache")

	val headers_1 = Map(
		"Accept" -> "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8",
		"Upgrade-Insecure-Requests" -> "1")

	val headers_10 = Map(
		"Accept" -> "application/json, text/javascript, */*; q=0.01",
		"X-Requested-With" -> "XMLHttpRequest")

	val headers_12 = Map(
		"Accept" -> "application/font-woff2;q=1.0,application/font-woff;q=0.9,*/*;q=0.8",
		"Accept-Encoding" -> "identity")

	val headers_25 = Map(
		"Accept" -> "text/javascript, text/html, application/xml, text/xml, */*",
		"Content-type" -> "application/x-www-form-urlencoded; charset=UTF-8",
		"X-Prototype-Version" -> "1.6.0.3",
		"X-Requested-With" -> "XMLHttpRequest")

    val uri1 = "medlearn-test2.medicine.arizona.edu"

	val scn = scenario("MedlearnTest2StudentLogin")
		.pause(3)
		.exec(http("login")
			.post("/?url=%2F")
			.headers(headers_1)
			.formParam("action", "login")
			.formParam("ssobypass", "1")
			.formParam("username", "BeeBopBoop")
			.formParam("password", "XXXXXXXXXX")
			.check(substring("The username or password you have provided is incorrect.").notExists)
			.check(substring("500 An internal server error has occurred.").notExists)
			.check(substring("429 An internal authentication error has occurred.").notExists)
			.check(substring("An internal authentication error has occurred.").notExists)
			.check(substring("error has occurred").notExists)
			.check(substring("empty response").notExists)
			.check(substring("Empty reply").notExists)
			.check(substring("error has occurred").count.saveAs("errorOccured"))			
			.check(substring("Message Center").exists)
			.check(bodyString.saveAs("loginPageResponse"))	
			)
			.doIf ( session => session("errorOccured").as[Int] > 0 ) {
				exec { session =>
					println(session("loginPageResponse").as[String])
					session
				}
			}

	setUp(scn.inject(rampUsers(280) over (120 seconds))).protocols(httpProtocol) //
	//setUp(scn.inject(atOnceUsers(30))).protocols(httpProtocol) //


