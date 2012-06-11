<%@ Page Language="C#" EnableTheming="false"
    AutoEventWireup="true" Codebehind="Credits.aspx.cs" Inherits="Subtext.Web.Admin.WebUI.Credits"
    Title="Subtext Admin - 3rd Party Credits" %>

<asp:Content ContentPlaceHolderID="actionsHeading" runat="server">
    <h2>Credits</h2>
</asp:Content>

<asp:Content ContentPlaceHolderID="categoryListHeading" runat="server" />

<asp:Content ContentPlaceHolderID="categoryListLinks" runat="server"></asp:Content>

<asp:Content ContentPlaceHolderID="pageContent" runat="server">
    <div id="credits">
        <h1>3rd Party Credits &amp; Licenses</h1>
        <p>
            SubText uses a number of third party open source components. Their details, copyrights
            and licenses are below.</p>
        <h2>Blowery HttpCompress</h2>
        <p>
            SubText uses a modified version of <a href="http://www.blowery.org/code/HttpCompressionModule.html">
                Blowery HttpCompress</a> to compress RSS feeds.</p>
        <blockquote>
            <p>
                Copyright (C) 2002-Present Ben Lowery (compress@blowery.org)</p>
            <p>
                This software is provided 'as-is', without any express or implied warranty. In no
                event will the authors be held liable for any damages arising from the use of this
                software.</p>
            <p>
                Permission is granted to anyone to use this software for any purpose, including
                commercial applications, and to alter it and redistribute it freely, subject to
                the following restrictions:</p>
            <ol>
                <li>The origin of this software must not be misrepresented; you must not claim that
                    you wrote the original software. If you use this software in a product, an acknowledgment
                    in the product documentation would be appreciated but is not required.</li>
                <li>Altered source versions must be plainly marked as such, and must not be misrepresented
                    as being the original software.</li>
                <li>This notice may not be removed or altered from any source distribution.</li>
            </ol>
            <p>
                Ben Lowery<br />
                compress@blowery.org</p>
        </blockquote>
        <h2>
            XML-RPC.NET</h2>
        <p>
            SubText uses Cook Computing's <a href="http://www.xml-rpc.net/">XML-RPC</a> to provide
            various API functions.</p>
        <blockquote>
            <p>
                The MIT License</p>
            <p>
                Copyright (c) 2006 Charles Cook</p>
            <p>
                Permission is hereby granted, free of charge, to any person obtaining a copy of
                this software and associated documentation files (the "Software"), to deal in the
                Software without restriction, including without limitation the rights to use, copy,
                modify, merge, publish, distribute, sublicense, and/or sell copies of the Software,
                and to permit persons to whom the Software is furnished to do so, subject to the
                following conditions:</p>
            <p>
                The above copyright notice and this permission notice shall be included in all copies
                or substantial portions of the Software.</p>
            <p>
                THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
                INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR
                PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS
                BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
                TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE
                USE OR OTHER DEALINGS IN THE SOFTWARE.</p>
        </blockquote>
        <h2>
            BlogML</h2>
        <p>
            SubText uses MarkItUp.com's <a href="http://codeplex.com/Wiki/View.aspx?ProjectName=BlogML">
                BlogML</a> to import and export your content.</p>
        <blockquote>
            <p>
                BlogML BSD License</p>
            <p>
                Copyright (c) 2006, MarkItUp.com</p>
            <p>
                All rights reserved.</p>
            <p>
                Redistribution and use in source and binary forms, with or without modification,
                are permitted provided that the following conditions are met:</p>
            <ul>
                <li>Redistributions of source code must retain the above copyright notice, this list
                    of conditions and the following disclaimer.</li>
                <li>Redistributions in binary form must reproduce the above copyright notice, this list
                    of conditions and the following disclaimer in the documentation and/or other materials
                    provided with the distribution.</li>
                <li>Neither the name of the MarkItUp.com nor the names of its contributors may be used
                    to endorse or promote products derived from this software without specific prior
                    written permission.</li>
            </ul>
            <p>
                THIS SOFTWARE IS PROVIDED BY THE MarkItUp.com AND CONTRIBUTORS ''AS IS'' AND ANY
                EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES
                OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT
                SHALL THE MarkItUp.com AND CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL,
                SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT
                OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION)
                HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY,
                OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF
                THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.</p>
        </blockquote>
        <h2>
            Log4Net</h2>
        <p>
            SubText uses the Apache Software Foundations's <a href="http://logging.apache.org/log4net/">
                Log4Net</a> to log errors and informational messages.</p>
        <blockquote>
            <pre class="code">
                                     Apache License
                               Version 2.0, January 2004
                            http://www.apache.org/licenses/

       TERMS AND CONDITIONS FOR USE, REPRODUCTION, AND DISTRIBUTION

       1. Definitions.

          &quot;License&quot; shall mean the terms and conditions for use, reproduction,
          and distribution as defined by Sections 1 through 9 of this document.

          &quot;Licensor&quot; shall mean the copyright owner or entity authorized by
          the copyright owner that is granting the License.

          &quot;Legal Entity&quot; shall mean the union of the acting entity and all
          other entities that control, are controlled by, or are under common
          control with that entity. For the purposes of this definition,
          &quot;control&quot; means (i) the power, direct or indirect, to cause the
          direction or management of such entity, whether by contract or
          otherwise, or (ii) ownership of fifty percent (50%) or more of the
          outstanding shares, or (iii) beneficial ownership of such entity.

          &quot;You&quot; (or &quot;Your&quot;) shall mean an individual or Legal Entity
          exercising permissions granted by this License.

          &quot;Source&quot; form shall mean the preferred form for making modifications,
          including but not limited to software source code, documentation
          source, and configuration files.

          &quot;Object&quot; form shall mean any form resulting from mechanical
          transformation or translation of a Source form, including but
          not limited to compiled object code, generated documentation,
          and conversions to other media types.

          &quot;Work&quot; shall mean the work of authorship, whether in Source or
          Object form, made available under the License, as indicated by a
          copyright notice that is included in or attached to the work
          (an example is provided in the Appendix below).

          &quot;Derivative Works&quot; shall mean any work, whether in Source or Object
          form, that is based on (or derived from) the Work and for which the
          editorial revisions, annotations, elaborations, or other modifications
          represent, as a whole, an original work of authorship. For the purposes
          of this License, Derivative Works shall not include works that remain
          separable from, or merely link (or bind by name) to the interfaces of,
          the Work and Derivative Works thereof.

          &quot;Contribution&quot; shall mean any work of authorship, including
          the original version of the Work and any modifications or additions
          to that Work or Derivative Works thereof, that is intentionally
          submitted to Licensor for inclusion in the Work by the copyright owner
          or by an individual or Legal Entity authorized to submit on behalf of
          the copyright owner. For the purposes of this definition, &quot;submitted&quot;

          means any form of electronic, verbal, or written communication sent
          to the Licensor or its representatives, including but not limited to
          communication on electronic mailing lists, source code control systems,
          and issue tracking systems that are managed by, or on behalf of, the
          Licensor for the purpose of discussing and improving the Work, but
          excluding communication that is conspicuously marked or otherwise
          designated in writing by the copyright owner as &quot;Not a Contribution.&quot;

          &quot;Contributor&quot; shall mean Licensor and any individual or Legal Entity
          on behalf of whom a Contribution has been received by Licensor and
          subsequently incorporated within the Work.

       2. Grant of Copyright License. Subject to the terms and conditions of
          this License, each Contributor hereby grants to You a perpetual,
          worldwide, non-exclusive, no-charge, royalty-free, irrevocable
          copyright license to reproduce, prepare Derivative Works of,
          publicly display, publicly perform, sublicense, and distribute the
          Work and such Derivative Works in Source or Object form.

       3. Grant of Patent License. Subject to the terms and conditions of
          this License, each Contributor hereby grants to You a perpetual,
          worldwide, non-exclusive, no-charge, royalty-free, irrevocable
          (except as stated in this section) patent license to make, have made,
          use, offer to sell, sell, import, and otherwise transfer the Work,
          where such license applies only to those patent claims licensable
          by such Contributor that are necessarily infringed by their
          Contribution(s) alone or by combination of their Contribution(s)
          with the Work to which such Contribution(s) was submitted. If You
          institute patent litigation against any entity (including a
          cross-claim or counterclaim in a lawsuit) alleging that the Work
          or a Contribution incorporated within the Work constitutes direct
          or contributory patent infringement, then any patent licenses
          granted to You under this License for that Work shall terminate
          as of the date such litigation is filed.

       4. Redistribution. You may reproduce and distribute copies of the
          Work or Derivative Works thereof in any medium, with or without
          modifications, and in Source or Object form, provided that You
          meet the following conditions:

          (a) You must give any other recipients of the Work or
              Derivative Works a copy of this License; and

          (b) You must cause any modified files to carry prominent notices
              stating that You changed the files; and

          (c) You must retain, in the Source form of any Derivative Works
              that You distribute, all copyright, patent, trademark, and
              attribution notices from the Source form of the Work,
              excluding those notices that do not pertain to any part of
              the Derivative Works; and

          (d) If the Work includes a &quot;NOTICE&quot; text file as part of its
              distribution, then any Derivative Works that You distribute must
              include a readable copy of the attribution notices contained
              within such NOTICE file, excluding those notices that do not
              pertain to any part of the Derivative Works, in at least one
              of the following places: within a NOTICE text file distributed
              as part of the Derivative Works; within the Source form or
              documentation, if provided along with the Derivative Works; or,
              within a display generated by the Derivative Works, if and
              wherever such third-party notices normally appear. The contents
              of the NOTICE file are for informational purposes only and
              do not modify the License. You may add Your own attribution
              notices within Derivative Works that You distribute, alongside
              or as an addendum to the NOTICE text from the Work, provided
              that such additional attribution notices cannot be construed
              as modifying the License.

          You may add Your own copyright statement to Your modifications and
          may provide additional or different license terms and conditions
          for use, reproduction, or distribution of Your modifications, or
          for any such Derivative Works as a whole, provided Your use,
          reproduction, and distribution of the Work otherwise complies with
          the conditions stated in this License.

       5. Submission of Contributions. Unless You explicitly state otherwise,
          any Contribution intentionally submitted for inclusion in the Work
          by You to the Licensor shall be under the terms and conditions of
          this License, without any additional terms or conditions.
          Notwithstanding the above, nothing herein shall supersede or modify
          the terms of any separate license agreement you may have executed
          with Licensor regarding such Contributions.

       6. Trademarks. This License does not grant permission to use the trade
          names, trademarks, service marks, or product names of the Licensor,
          except as required for reasonable and customary use in describing the
          origin of the Work and reproducing the content of the NOTICE file.

       7. Disclaimer of Warranty. Unless required by applicable law or
          agreed to in writing, Licensor provides the Work (and each
          Contributor provides its Contributions) on an &quot;AS IS&quot; BASIS,
          WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or
          implied, including, without limitation, any warranties or conditions
          of TITLE, NON-INFRINGEMENT, MERCHANTABILITY, or FITNESS FOR A
          PARTICULAR PURPOSE. You are solely responsible for determining the
          appropriateness of using or redistributing the Work and assume any
          risks associated with Your exercise of permissions under this License.

       8. Limitation of Liability. In no event and under no legal theory,
          whether in tort (including negligence), contract, or otherwise,
          unless required by applicable law (such as deliberate and grossly
          negligent acts) or agreed to in writing, shall any Contributor be
          liable to You for damages, including any direct, indirect, special,
          incidental, or consequential damages of any character arising as a
          result of this License or out of the use or inability to use the
          Work (including but not limited to damages for loss of goodwill,
          work stoppage, computer failure or malfunction, or any and all
          other commercial damages or losses), even if such Contributor
          has been advised of the possibility of such damages.

       9. Accepting Warranty or Additional Liability. While redistributing
          the Work or Derivative Works thereof, You may choose to offer,
          and charge a fee for, acceptance of support, warranty, indemnity,
          or other liability obligations and/or rights consistent with this
          License. However, in accepting such obligations, You may act only
          on Your own behalf and on Your sole responsibility, not on behalf
          of any other Contributor, and only if You agree to indemnify,
          defend, and hold each Contributor harmless for any liability
          incurred by, or claims asserted against, such Contributor by reason
          of your accepting any such warranty or additional liability.

       END OF TERMS AND CONDITIONS

       APPENDIX: How to apply the Apache License to your work.

          To apply the Apache License to your work, attach the following
          boilerplate notice, with the fields enclosed by brackets &quot;[]&quot;

          replaced with your own identifying information. (Don't include
          the brackets!)  The text should be enclosed in the appropriate
          comment syntax for the file format. We also recommend that a
          file or class name and description of purpose be included on the
          same &quot;printed page&quot; as the copyright notice for easier
          identification within third-party archives.

       Copyright [yyyy] [name of copyright owner]

       Licensed under the Apache License, Version 2.0 (the &quot;License&quot;);
       you may not use this file except in compliance with the License.
       You may obtain a copy of the License at

           http://www.apache.org/licenses/LICENSE-2.0

       Unless required by applicable law or agreed to in writing, software
       distributed under the License is distributed on an &quot;AS IS&quot; BASIS,
       WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
       See the License for the specific language governing permissions and
       limitations under the License.</pre>
        </blockquote>
        <h2>
            DotNetOpenMail</h2>
        <p>
            SubText used <a href="http://sourceforge.net/projects/dotnetopenmail/">DotNetOpenMail</a>
            to provide email services under v1.1 of the .NET framework.</p>
        <blockquote>
            <p>
                Copyright (c) 2005 Mike Bridge &lt;mike@bridgecanada.com&gt;</p>
            <p>
                Permission is hereby granted, free of charge, to any person obtaining a copy of
                this software and associated documentation files (the "Software"), to deal in the
                Software without restriction, including without limitation the rights to use, copy,
                modify, merge, publish, distribute, sublicense, and/or sell copies of the Software,
                and to permit persons to whom the Software is furnished to do so, subject to the
                following conditions:</p>
            <p>
                The above copyright notice and this permission notice shall be included in all copies
                or substantial portions of the Software.</p>
            <p>
                THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
                INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR
                PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS
                BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
                TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE
                USE OR OTHER DEALINGS IN THE SOFTWARE.</p>
        </blockquote>
        <h2>
            SgmlReaderXML</h2>
        <p>
            Subtext uses Microsoft's <a href="http://www.gotdotnet.com/Community/UserSamples/Details.aspx?SampleGuid=B90FDDCE-E60D-43F8-A5C4-C3BD760564BC">
                SgmlReaderXML</a> for converting HTML to XHTML (for example in comments).</p>
        <blockquote>
            <p>
                End User License Agreement</p>
            <p>
                USE OF THE SOFTWARE YOU ARE ABOUT TO DOWNLOAD IS GOVERNED BY THE TERMS OF THE END
                USER LICENSE AGREEMENT (“EULA”), IF ANY, WHICH ACCOMPANIES OR IS INCLUDED WITH SUCH
                SOFTWARE. IN THE EVENT THE SOFTWARE IS NOT ACCOMPANIED BY A EULA, USE SHALL BE GOVERNED
                BY THE FOLLOWING LICENSE TERMS:</p>
            <p>
                <b>IMPORTANT—READ CAREFULLY:</b> This End-User License Agreement (“EULA”) is a legal
                agreement between you (either an individual or a single entity) and Microsoft Corporation
                for the Microsoft software that accompanies this EULA, which includes computer software
                and may include associated media, printed materials, “online” or electronic documentation,
                and Internet-based services (“Software”). An amendment or addendum to this EULA
                may accompany the Software. YOU AGREE TO BE BOUND BY THE TERMS OF THIS EULA BY INSTALLING,
                COPYING, OR OTHERWISE USING THE SOFTWARE. IF YOU DO NOT AGREE, DO NOT INSTALL, COPY,
                OR USE THE SOFTWARE.
            </p>
            <ol>
                <li><b>GRANT OF LICENSE.</b> Microsoft grants you the following rights provided that
                    you comply with all terms and conditions of this EULA:
                    <ol style="list-style-type: lower-alpha">
                        <li>Microsoft grants you a personal, nonexclusive, royalty-free license to install and
                            use the Software for design, development, and testing purposes. You may install
                            and use the Software on an unlimited number of computers so long as you are the
                            only individual using the Software. </li>
                        <li>Solely with respect to the sample source code contained in the Software (“MS Samples”),
                            Microsoft also grants you a limited, nonexclusive, royalty-free license to: (a)
                            use and modify the source code version of those portions of the MS Samples for the
                            sole purposes of designing, developing, and testing your software product(s), and
                            (b) to reproduce and distribute the MS Samples, along with any modifications thereof,
                            in object and/or source code form. For applicable redistribution requirements for
                            MS Samples, see Section 2 below. </li>
                    </ol>
                </li>
                <li><b>DESCRIPTION OF OTHER RIGHTS AND LIMITATIONS </b>
                    <ol style="list-style-type: lower-alpha">
                        <li>If you choose to redistribute the MS Samples, you agree: (i) to distribute the MS
                            Samples only as a part of a software application product developed by you (“Licensee
                            Software”); (ii) not to use Microsoft’s name, logo, or trademarks to market the
                            Licensee Software; (iii) to display your own valid copyright notice which shall
                            be sufficient to protect Microsoft’s copyright in the MS Samples; (iv) to indemnify,
                            hold harmless, and defend Microsoft from and against any claims or lawsuits, including
                            attorney’s fees, that arise or result from the use or distribution of the Licensee
                            Software; (v) not to permit further distribution of the MS Samples by your end users;
                            and (vi) that Microsoft reserves all rights not expressly granted. </li>
                        <li>If you use the MS Samples, then the following also applies. Your license rights
                            to the MS Samples are conditioned upon your (i) not incorporating Identified Software
                            into, or combining Identified Software with, the MS Samples or a derivative work
                            thereof; and (ii) not distributing Identified Software in conjunction with the MS
                            Samples or a derivative work thereof. “Identified Software” means software which
                            is licensed pursuant to terms that directly or indirectly (A) create, or purport
                            to create, obligations for Microsoft with respect to the MS Samples or derivative
                            work thereof or (B) grant, or purport to grant, to any third party any rights or
                            immunities under Microsoft’s intellectual property or proprietary rights in the
                            MS Samples or derivative work thereof. Identified Software includes, without limitation,
                            any software that requires as a condition of its use, modification and/or distribution,
                            that any other software incorporated into, derived from or distributed with such
                            software must also be (1) disclosed or distributed in source code form; (2) licensed
                            for the purpose of making derivative works; or (3) redistributable at no charge.
                        </li>
                    </ol>
                </li>
                <li><b>RESERVATION OF RIGHTS AND OWNERSHIP.</b> Microsoft reserves all rights not expressly
                    granted to you in this EULA. The Software is protected by copyright and other intellectual
                    property laws and treaties. Microsoft or its suppliers own the title, copyright,
                    and other intellectual property rights in the Software. The Software is licensed,
                    not sold.</li>
                <li><b>LIMITATIONS ON REVERSE ENGINEERING, DECOMPILATION, AND DISASSEMBLY.</b> You may
                    not reverse engineer, decompile, or disassemble the Software, except and only to
                    the extent that such activity is expressly permitted by applicable law notwithstanding
                    this limitation.</li>
                <li><b>SUPPORT SERVICES.</b> No technical support will be provided for the Software.</li>
                <li><b>LINKS TO THIRD PARTY SITES.</b> You may link to third party sites through the
                    use of the Software. The third party sites are not under the control of Microsoft,
                    and Microsoft is not responsible for the contents of any third party sites, any
                    links contained in third party sites, or any changes or updates to third party sites.
                    Microsoft is not responsible for webcasting or any other form of transmission received
                    from any third party sites. Microsoft is providing these links to third party sites
                    to you only as a convenience, and the inclusion of any link does not imply an endorsement
                    by Microsoft of the third party site.</li>
                <li><b>ADDITIONAL SOFTWARE/SERVICES.</b> This EULA applies to updates, supplements,
                    add-on components, or Internet-based services components, of the Software that Microsoft
                    may provide to you or make available to you after the date you obtain your initial
                    copy of the Software, unless we provide other terms along with the update, supplement,
                    add-on component, or Internet-based services component. Microsoft reserves the right
                    to discontinue any Internet-based services provided to you or made available to
                    you through the use of the Software.</li>
                <li><b>U.S. GOVERNMENT LICENSE RIGHTS.</b> All Software provided to the U.S. Government
                    pursuant to solicitations issued on or after December 1, 1995 is provided with the
                    commercial license rights and restrictions described elsewhere herein. All Software
                    provided to the U.S. Government pursuant to solicitations issued prior to December
                    1, 1995 is provided with “Restricted Rights” as provided for in FAR, 48 CFR 52.227-14
                    (JUNE 1987) or DFAR, 48 CFR 252.227-7013 (OCT 1988), as applicable.</li>
                <li><b>EXPORT RESTRICTIONS.</b> You acknowledge that the Software is subject to U.S.
                    export jurisdiction. You agree to comply with all applicable international and national
                    laws that apply to the Software, including the U.S. Export Administration Regulations,
                    as well as end-user, end-use, and destination restrictions issued by U.S. and other
                    governments. For additional information see <a href="http://www.microsoft.com/exporting/">
                        http://www.microsoft.com/exporting</a>.</li>
                <li><b>SOFTWARE TRANSFER.</b> The initial user of the Software may make a one-time permanent
                    transfer of this EULA and Software to another end user, provided the initial user
                    retains no copies of the Software. The transfer may not be an indirect transfer,
                    such as a consignment. Prior to the transfer, the end user receiving the Software
                    must agree to all the EULA terms.</li>
                <li><b>TERMINATION.</b> Without prejudice to any other rights, Microsoft may terminate
                    this EULA if you fail to comply with the terms and conditions of this EULA. In such
                    event, you must destroy all copies of the Software and all of its component parts.</li>
                <li><b>DISCLAIMER OF WARRANTIES.</b> TO THE MAXIMUM EXTENT PERMITTED BY APPLICABLE LAW,
                    MICROSOFT AND ITS SUPPLIERS PROVIDE TO YOU THE SOFTWARE, AND SUPPORT SERVICES (IF
                    ANY) AS IS AND WITH ALL FAULTS; AND MICROSOFT AND ITS SUPPLIERS HEREBY DISCLAIM
                    ALL OTHER WARRANTIES AND CONDITIONS, WHETHER EXPRESS, IMPLIED OR STATUTORY, INCLUDING,
                    BUT NOT LIMITED TO, ANY (IF ANY) IMPLIED WARRANTIES, DUTIES OR CONDITIONS OF MERCHANTABILITY,
                    OF FITNESS FOR A PARTICULAR PURPOSE, OF RELIABILITY OR AVAILABILITY, OF ACCURACY
                    OR COMPLETENESS OF RESPONSES, OF RESULTS, OF WORKMANLIKE EFFORT, OF LACK OF VIRUSES,
                    AND OF LACK OF NEGLIGENCE, ALL WITH REGARD TO THE SOFTWARE, AND THE PROVISION OF
                    OR FAILURE TO PROVIDE SUPPORT OR OTHER SERVICES, INFORMATION, SOFTWARE, AND RELATED
                    CONTENT THROUGH THE SOFTWARE OR OTHERWISE ARISING OUT OF THE USE OF THE SOFTWARE.
                    ALSO, THERE IS NO WARRANTY OR CONDITION OF TITLE, QUIET ENJOYMENT, QUIET POSSESSION,
                    CORRESPONDENCE TO DESCRIPTION OR NON-INFRINGEMENT WITH REGARD TO THE SOFTWARE.</li>
                <li><b>EXCLUSION OF INCIDENTAL, CONSEQUENTIAL AND CERTAIN OTHER DAMAGES.</b> TO THE
                    MAXIMUM EXTENT PERMITTED BY APPLICABLE LAW, IN NO EVENT SHALL MICROSOFT OR ITS SUPPLIERS
                    BE LIABLE FOR ANY SPECIAL, INCIDENTAL, PUNITIVE, INDIRECT, OR CONSEQUENTIAL DAMAGES
                    WHATSOEVER (INCLUDING, BUT NOT LIMITED TO, DAMAGES FOR LOSS OF PROFITS OR CONFIDENTIAL
                    OR OTHER INFORMATION, FOR BUSINESS INTERRUPTION, FOR PERSONAL INJURY, FOR LOSS OF
                    PRIVACY, FOR FAILURE TO MEET ANY DUTY INCLUDING OF GOOD FAITH OR OF REASONABLE CARE,
                    FOR NEGLIGENCE, AND FOR ANY OTHER PECUNIARY OR OTHER LOSS WHATSOEVER) ARISING OUT
                    OF OR IN ANY WAY RELATED TO THE USE OF OR INABILITY TO USE THE PRODUCT, THE PROVISION
                    OF OR FAILURE TO PROVIDE SUPPORT OR OTHER SERVICES, INFORMATON, SOFTWARE, AND RELATED
                    CONTENT THROUGH THE PRODUCT OR OTHERWISE ARISING OUT OF THE USE OF THE PRODUCT,
                    OR OTHERWISE UNDER OR IN CONNECTION WITH ANY PROVISION OF THIS EULA, EVEN IN THE
                    EVENT OF THE FAULT, TORT (INCLUDING NEGLIGENCE), MISREPRESENTATION, STRICT LIABILITY,
                    BREACH OF CONTRACT OR BREACH OF WARRANTY OF MICROSOFT OR ANY SUPPLIER, AND EVEN
                    IF MICROSOFT OR ANY SUPPLIER HAS BEEN ADVISED OF THE POSSIBILITY OF SUCH DAMAGES.
                </li>
                <li><b>LIMITATION OF LIABILITY AND REMEDIES.</b> NOTWITHSTANDING ANY DAMAGES THAT YOU
                    MIGHT INCUR FOR ANY REASON WHATSOEVER (INCLUDING, WITHOUT LIMITATION, ALL DAMAGES
                    REFERENCED ABOVE AND ALL DIRECT OR GENERAL DAMAGES IN CONTRACT OR ANYTHING ELSE),
                    THE ENTIRE LIABILITY OF MICROSOFT AND ANY OF ITS SUPPLIERS UNDER ANY PROVISION OF
                    THIS SUPPLEMENTAL EULA AND YOUR EXCLUSIVE REMEDY FOR ALL OF THE FOREGOING SHALL
                    BE LIMITED TO THE GREATER OF THE ACTUAL DAMAGES YOU INCUR IN REASONABLE RELIANCE
                    ON THE SOFTWARE UP TO THE AMOUNT ACTUALLY PAID BY YOU FOR THE SOFTWARE OR U.S.$5.00.
                    THE FOREGOING LIMITATIONS, EXCLUSIONS AND DISCLAIMERS SHALL APPLY TO THE MAXIMUM
                    EXTENT PERMITTED BY APPLICABLE LAW, EVEN IF ANY REMEDY FAILS ITS ESSENTIAL PURPOSE.</li>
                <li><b>APPLICABLE LAW.</b> If you acquired this Software in the United States, this
                    EULA is governed by the laws of the State of Washington. If you acquired this Software
                    in Canada, unless expressly prohibited by local law, this EULA is governed by the
                    laws in force in the Province of Ontario, Canada; and, in respect of any dispute
                    which may arise hereunder, you consent to the jurisdiction of the federal and provincial
                    courts sitting in Toronto, Ontario. If you acquired this Software in the Eurpean
                    Union, Iceland, Norway, or Switzerland, then local law applies. If you acquired
                    this Software in any other country, then local law may apply. </li>
                <li><b>ENTIRE AGREEMENT; SEVERABILITY.</b> This EULA (including any addendum or amendment
                    to this EULA which is included with the Software) are the entire agreement between
                    you and Microsoft relating to the Software and the support services (if any) and
                    they supersede all prior or contemporaneous oral or written communications, proposals
                    and representations with respect to the Software or any other subject matter covered
                    by this EULA. To the extent the terms of any Microsoft policies or programs for
                    support services conflict with the terms of this EULA, the terms of this EULA shall
                    control. If any provision of this EULA is held to be void, invalid, unenforceable
                    or illegal, the other provisions shall continue in full force and effect.</li>
            </ol>
            <p>
                Si vous avez acquis votre logiciel Microsoft au CANADA :<br />
            </p>
            <p>
                <b>DÉNI DE GARANTIES.</b> Dans la mesure maximale permise par les lois applicables,
                le Logiciel et les services de soutien technique (le cas échéant) sont fournis TELS
                QUELS ET AVEC TOUS LES DÉFAUTS par Microsoft et ses fournisseurs, lesquels par les
                présentes dénient toutes autres garanties et conditions expresses, implicites ou
                en vertu de la loi, notamment, mais sans limitation, (le cas échéant) les garanties,
                devoirs ou conditions implicites de qualité marchande, d’adaptation à une fin usage
                particulièere, de fiabilité ou de disponibilité, d’exactitude ou d’exhaustivité
                des réponses, des résultats, des efforts déployés selon les règles de l’art, d’absence
                de virus et d’absence de négligence, le tout à l’égard du Logiciel et de la prestation
                des services de soutien technique ou de l’omission de la ’une telle prestation des
                services de soutien technique ou à l’égard de la fourniture ou de l’omission de
                la fourniture de tous autres services, renseignements, logiciels, et contenu qui
                s’y rapporte grâce au Logiciel ou provenant autrement de l’utilisation du Logiciel
                . PAR AILLEURS, IL N’Y A AUCUNE GARANTIE OU CONDITION QUANT AU TITRE DE PROPRIÉTÉ,
                À LA JOUISSANCE OU LA POSSESSION PAISIBLE, À LA CONCORDANCE À UNE DESCRIPTION NI
                QUANT À UNE ABSENCE DE CONTREFAÇON CONCERNANT LE LOGICIEL.
            </p>
            <p>
                <b>EXCLUSION DES DOMMAGES ACCESSOIRES, INDIRECTS ET DE CERTAINS AUTRES DOMMAGES.</b>
                DANS LA MESURE MAXIMALE PERMISE PAR LES LOIS APPLICABLES, EN AUCUN CAS MICROSOFT
                OU SES FOURNISSEURS NE SERONT RESPONSABLES DES DOMMAGES SPÉCIAUX, CONSÉCUTIFS, ACCESSOIRES
                OU INDIRECTS DE QUELQUE NATURE QUE CE SOIT (NOTAMMENT, LES DOMMAGES À L’ÉGARD DU
                MANQUE À GAGNER OU DE LA DIVULGATION DE RENSEIGNEMENTS CONFIDENTIELS OU AUTRES,
                DE LA PERTE D’EXPLOITATION, DE BLESSURES CORPORELLES, DE LA VIOLATION DE LA VIE
                PRIVÉE, DE L’OMISSION DE REMPLIR TOUT DEVOIR, Y COMPRIS D’AGIR DE BONNE FOI OU D’EXERCER
                UN SOIN RAISONNABLE, DE LA NÉGLIGENCE ET DE TOUTE AUTRE PERTE PÉCUNIAIRE OU AUTRE
                PERTE DE QUELQUE NATURE QUE CE SOIT) SE RAPPORTANT DE QUELQUE MANIÈRE QUE CE SOIT
                À L’UTILISATION DU LOGICIEL OU À L’INCAPACITÉ DE S’EN SERVIR, À LA PRESTATION OU
                À L’OMISSION DE LA ’UNE TELLE PRESTATION DE SERVICES DE SOUTIEN TECHNIQUE OU À LA
                FOURNITURE OU À L’OMISSION DE LA FOURNITURE DE TOUS AUTRES SERVICES, RENSEIGNEMENTS,
                LOGICIELS, ET CONTENU QUI S’Y RAPPORTE GRÂCE AU LOGICIEL OU PROVENANT AUTREMENT
                DE L’UTILISATION DU LOGICIEL OU AUTREMENT AUX TERMES DE TOUTE DISPOSITION DE LA
                U PRÉSENTE CONVENTION EULA OU RELATIVEMENT À UNE TELLE DISPOSITION, MÊME EN CAS
                DE FAUTE, DE DÉLIT CIVIL (Y COMPRIS LA NÉGLIGENCE), DE RESPONSABILITÉ STRICTE, DE
                VIOLATION DE CONTRAT OU DE VIOLATION DE GARANTIE DE MICROSOFT OU DE TOUT FOURNISSEUR
                ET MÊME SI MICROSOFT OU TOUT FOURNISSEUR A ÉTÉ AVISÉ DE LA POSSIBILITÉ DE TELS DOMMAGES.
            </p>
            <p>
                <b>LIMITATION DE RESPONSABILITÉ ET RECOURS.</b> MALGRÉ LES DOMMAGES QUE VOUS PUISSIEZ
                SUBIR POUR QUELQUE MOTIF QUE CE SOIT (NOTAMMENT, MAIS SANS LIMITATION, TOUS LES
                DOMMAGES SUSMENTIONNÉS ET TOUS LES DOMMAGES DIRECTS OU GÉNÉRAUX OU AUTRES), LA SEULE
                RESPONSABILITÉ ’OBLIGATION INTÉGRALE DE MICROSOFT ET DE L’UN OU L’AUTRE DE SES FOURNISSEURS
                AUX TERMES DE TOUTE DISPOSITION DEU LA PRÉSENTE CONVENTION EULA ET VOTRE RECOURS
                EXCLUSIF À L’ÉGARD DE TOUT CE QUI PRÉCÈDE SE LIMITE AU PLUS ÉLEVÉ ENTRE LES MONTANTS
                SUIVANTS : LE MONTANT QUE VOUS AVEZ RÉELLEMENT PAYÉ POUR LE LOGICIEL OU 5,00 $US.
                LES LIMITES, EXCLUSIONS ET DÉNIS QUI PRÉCÈDENT (Y COMPRIS LES CLAUSES CI-DESSUS),
                S’APPLIQUENT DANS LA MESURE MAXIMALE PERMISE PAR LES LOIS APPLICABLES, MÊME SI TOUT
                RECOURS N’ATTEINT PAS SON BUT ESSENTIEL.
            </p>
            <p>
                À moins que cela ne soit prohibé par le droit local applicable, la présente Convention
                est régie par les lois de la province d’Ontario, Canada. Vous consentez Chacune
                des parties à la présente reconnaît irrévocablement à la compétence des tribunaux
                fédéraux et provinciaux siégeant à Toronto, dans de la province d’Ontario et consent
                à instituer tout litige qui pourrait découler de la présente auprès des tribunaux
                situés dans le district judiciaire de York, province d’Ontario.</p>
            <p>
                Au cas où vous auriez des questions concernant cette licence ou que vous désiriez
                vous mettre en rapport avec Microsoft pour quelque raison que ce soit, veuillez
                utiliser l’information contenue dans le Logiciel pour contacter la filiale de succursale
                Microsoft desservant votre pays, dont l’adresse est fournie dans ce produit, ou
                visitez écrivez à : Microsoft sur le World Wide Web à <a href="http://www.microsoft.com">
                    http://www.microsoft.com</a> Sales Information Center, One Microsoft Way, Redmond,
                Washington 98052-6399.</p>
        </blockquote>
        <h2>DotNetZip</h2>
        <p>
            SubText links to <a href="http://dotnetzip.codeplex.com/">DotNetZip</a> to provide zipping and unzipping functionality in various places.
        </p>
        <p>License</p>
        <p>
            The library is released under the <a href="http://dotnetzip.codeplex.com/license">Ms-PL</a> 
            license.
        </p>
        <h2>FCKeditor</h2>
        <p>
            SubText links to <a href="http://www.fckeditor.net/">FCKeditor</a> to provide a
            rich text editor to add your blog posts via the Admin interface.</p>
        <blockquote>
            <p>
                The editor is distributed under the <a href="http://www.gnu.org/licenses/gpl.html">GPL</a>,
                <a href="http://www.gnu.org/licenses/lgpl.html">LGPL</a> and <a href="http://www.mozilla.org/MPL/MPL-1.1.html">
                    MPL</a> open source licenses. This triple licensing model avoids incompatibility
                with other open source licenses, making it possible to integrate FCKeditor whenever
                you want.</p>
        </blockquote>
        <h2>
            FreeTextBox</h2>
        <p>
            Subtext uses <a href="http://freetextbox.com/default.aspx">FreeTextBox</a> as an
            alternative for FCKeditor.</p>
   </div>
</asp:Content>
